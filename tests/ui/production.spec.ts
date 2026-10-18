import {test,expect,Page} from '@playwright/test';

const u='/?case=career_offer';
async function reset(page:Page,xp=220){await page.goto(u);await page.evaluate(({xp})=>{localStorage.clear();localStorage.setItem('daria.game.xp',String(xp));localStorage.setItem('daria.game.streak','0');localStorage.setItem('daria.game.history','[]')},{xp});await page.reload()}
async function noOverflow(page:Page){const x=await page.evaluate(()=>({w:innerWidth,sw:document.documentElement.scrollWidth,bw:document.body.scrollWidth}));expect(Math.max(x.sw,x.bw)).toBeLessThanOrEqual(x.w+1)}
async function playQuickThreeMoves(page:Page){await page.goto(u);await page.getByRole('button',{name:/Enter case/}).click();for(const a of ['Ask','Check','Compare']){await page.locator('.actionCard').filter({hasText:a}).click();await expect(page.getByText(/NEW INFORMATION/)).toBeVisible();if(a!=='Compare')await page.getByRole('button',{name:/Keep digging/}).click()}await page.getByRole('button',{name:/Decide now/}).click()}

test('production is really served over HTTP',async({page})=>{const errors:string[]=[];page.on('pageerror',e=>errors.push(e.message));const r=await page.goto(u);expect(r?.ok()).toBeTruthy();expect(new URL(page.url()).protocol).toBe('http:');await expect(page.locator('meta[name="daria-build"]')).toHaveAttribute('content','3.8.0');await expect(page.locator('#homeCaseTitle')).not.toHaveText('Something doesn’t add up.');expect(errors).toEqual([])});
test('home is a game opening, not a progression dashboard',async({page})=>{
  await page.goto(u);
  await expect(page.getByRole('button',{name:/Enter case/})).toBeVisible();
  await expect(page.getByRole('button',{name:/Investigation/})).toBeVisible();
  await expect(page.getByText(/Reasoning skill/)).toHaveCount(0);
  await expect(page.getByText(/XP/)).toHaveCount(0);
  await expect(page.getByText(/Compare · 80 XP/)).toHaveCount(0);
  await expect(page.locator('#homeCaseTitle')).toHaveText(/.+/);
});
test('mobile first viewport contains the primary play action',async({page})=>{
  await page.setViewportSize({width:390,height:844});
  await page.goto(u);
  const b=await page.getByRole('button',{name:/Enter case/}).boundingBox();
  expect(b).toBeTruthy();
  expect(b!.y+b!.height).toBeLessThanOrEqual(844);
});
test('home case continues into the selected mode',async({page})=>{
  await page.goto('/?case=activation');
  await expect(page.locator('#homeCaseTitle')).toHaveText('Everyone blames onboarding');
  await page.getByRole('button',{name:/Enter case/}).click();
  await expect(page.locator('#quickTitle')).toHaveText('Everyone blames onboarding');
});
test('corrupted history does not brick boot',async({page})=>{await page.addInitScript(()=>localStorage.setItem('daria.game.history','{broken'));await page.goto(u);await expect(page.getByRole('button',{name:/Enter case/})).toBeVisible()});
test('Quick Play is a case board with no answer hints',async({page})=>{await reset(page);await page.goto(u);await page.getByRole('button',{name:/Enter case/}).click();await expect(page.locator('.actionCard')).toHaveCount(4);await expect(page.getByText(/Could materially change/)).toHaveCount(0);await expect(page.getByText(/High information/)).toHaveCount(0);await expect(page.getByText(/Commits before/)).toHaveCount(0)});
test('Quick Play reveals consequences without grading the move',async({page})=>{await reset(page);await page.goto(u);await page.getByRole('button',{name:/Enter case/}).click();await page.locator('.actionCard').filter({hasText:'Ask'}).click();await expect(page.getByText(/NEW INFORMATION/)).toBeVisible();await expect(page.getByText(/Good\.|Not useless|High information|low commitment/)).toHaveCount(0);await expect(page.getByRole('button',{name:/Decide now/})).toBeVisible()});
test('quick case can end early and persists result',async({page})=>{await reset(page);await page.goto(u);await page.getByRole('button',{name:/Enter case/}).click();await page.locator('.actionCard').filter({hasText:'Check'}).click();await page.getByRole('button',{name:/Decide now/}).click();await page.locator('.quickCall').first().click();await expect(page.locator('#result')).toHaveClass(/active/);const h=await page.evaluate(()=>localStorage.getItem('daria.game.history'));await page.reload();expect(await page.evaluate(()=>localStorage.getItem('daria.game.history'))).toBe(h)});
test('quick actions are non-repeatable and clues accumulate',async({page})=>{await reset(page);await page.goto(u);await page.getByRole('button',{name:/Enter case/}).click();await page.locator('.actionCard').filter({hasText:'Ask'}).click();await page.getByRole('button',{name:/Keep digging/}).click();await expect(page.locator('.actionCard').filter({hasText:'Ask'})).toBeDisabled();await expect(page.locator('.clueSlot.filled')).toHaveCount(1)});
test('quick play supports full move budget then final call',async({page})=>{await reset(page);await playQuickThreeMoves(page);await expect(page.locator('.quickCall')).toHaveCount(4)});
test('investigation exposes four tools after unlock',async({page})=>{await reset(page);await page.goto(u);await page.getByRole('button',{name:/Investigation/}).click();for(const n of ['Ask','Check','Compare','Test'])await expect(page.getByRole('button',{name:n,exact:true})).toBeEnabled()});
test('evidence gates the next move',async({page})=>{await reset(page);await page.goto(u);await page.getByRole('button',{name:/Investigation/}).click();await page.getByRole('button',{name:'Ask',exact:true}).click();await expect(page.locator('#evidenceCard')).toBeVisible();await expect(page.getByRole('button',{name:'Check',exact:true})).toBeDisabled();await page.getByRole('button',{name:/Supports/}).click();await page.waitForTimeout(700);await expect(page.getByRole('button',{name:'Check',exact:true})).toBeEnabled()});
test('swipe right classifies support',async({page})=>{await reset(page);await page.goto(u);await page.getByRole('button',{name:/Investigation/}).click();await page.getByRole('button',{name:'Ask',exact:true}).click();const c=page.locator('#evidenceCard'),b=await c.boundingBox();expect(b).toBeTruthy();await page.mouse.move(b!.x+30,b!.y+50);await page.mouse.down();await page.mouse.move(b!.x+150,b!.y+50,{steps:5});await page.mouse.up();await expect(page.getByText(/Good read/)).toBeVisible()});
test('contradiction changes visual state',async({page})=>{await reset(page);await page.goto('/?case=activation');await page.getByRole('button',{name:/Investigation/}).click();await page.getByRole('button',{name:'Test',exact:true}).click();await expect(page.locator('#evidenceCard')).toHaveClass(/contradiction/);await expect(page.locator('#invChar')).toHaveAttribute('data-state','contradiction')});
test('early Make a call is allowed',async({page})=>{await reset(page);await page.goto(u);await page.getByRole('button',{name:/Investigation/}).click();await page.getByRole('button',{name:/Make a call/}).click();await expect(page.locator('.call')).toHaveCount(4);await page.locator('.call').first().click();await expect(page.locator('#result')).toHaveClass(/active/)});

for(const [name,w,h] of [['320',320,700],['360',360,780],['390',390,844],['430',430,932],['landscape',844,390],['short',390,430]] as const){
  test(name+' has no horizontal overflow',async({page})=>{await reset(page);await page.setViewportSize({width:w,height:h});await page.goto(u);await noOverflow(page);await page.getByRole('button',{name:/Investigation/}).click();await noOverflow(page)})
}
test('mobile buttons meet 44px target',async({page})=>{await page.setViewportSize({width:390,height:844});await page.goto(u);const hs=await page.locator('button:visible').evaluateAll(es=>es.map((e:any)=>e.getBoundingClientRect().height));for(const h of hs)expect.soft(h).toBeGreaterThanOrEqual(44)});
test('reduced motion keeps quick reveal static',async({page})=>{await page.emulateMedia({reducedMotion:'reduce'});await page.goto(u);await page.getByRole('button',{name:/Enter case/}).click();await page.locator('.actionCard').first().click();const d=await page.locator('.quickReveal').evaluate(e=>parseFloat(getComputedStyle(e).animationDuration)||0);expect(d).toBeLessThan(.01)});
test('keyboard focus is visible',async({page})=>{await page.goto(u);await page.keyboard.press('Tab');const ok=await page.evaluate(()=>{const e=document.activeElement as HTMLElement;return e?.tagName==='BUTTON'&&getComputedStyle(e).outlineStyle!=='none'});expect(ok).toBeTruthy()});
test('capture canonical visual states',async({page})=>{await reset(page);await page.goto(u);await page.screenshot({path:'artifacts/v37-home.png',fullPage:true});await page.getByRole('button',{name:/Investigation/}).click();await page.getByRole('button',{name:'Ask',exact:true}).click();await page.screenshot({path:'artifacts/v37-investigation-evidence.png',fullPage:true})});


test('debrief reconstructs the consequence chain',async({page})=>{
  await reset(page);
  await page.goto(u);
  await page.getByRole('button',{name:/Enter case/}).click();
  await page.locator('.actionCard').first().click();
  await page.getByRole('button',{name:/Decide now/}).click();
  await page.locator('.quickCall').first().click();
  await expect(page.locator('#consequenceChain .consequenceStep')).toHaveCount(2);
  await expect(page.locator('#consequenceChain')).toContainText(/Your call/);
});

test('completion and restart emit measurable events',async({page})=>{
  await reset(page);
  await page.goto(u);
  await page.getByRole('button',{name:/Enter case/}).click();
  await page.locator('.actionCard').first().click();
  await page.getByRole('button',{name:/Decide now/}).click();
  await page.locator('.quickCall').first().click();
  const complete=await page.evaluate(()=>JSON.parse(localStorage.getItem('daria.telemetry')||'[]').some((x:any)=>x.event==='case_complete'));
  expect(complete).toBeTruthy();
  await page.getByRole('button',{name:/Play another/}).click();
  const restarted=await page.evaluate(()=>JSON.parse(localStorage.getItem('daria.telemetry')||'[]').some((x:any)=>x.event==='case_restart'));
  expect(restarted).toBeTruthy();
});

test('share has a graceful clipboard fallback',async({page,context,browserName})=>{
  test.skip(browserName!=='chromium','clipboard fallback is asserted on Chromium');
  await context.grantPermissions(['clipboard-read','clipboard-write']);
  await page.addInitScript(()=>Object.defineProperty(navigator,'share',{value:undefined,configurable:true}));
  await reset(page);
  await page.goto(u);
  await page.getByRole('button',{name:/Enter case/}).click();
  await page.locator('.actionCard').first().click();
  await page.getByRole('button',{name:/Decide now/}).click();
  await page.locator('.quickCall').first().click();
  await page.getByRole('button',{name:/Share result/}).click();
  await expect(page.getByRole('button',{name:/Copied/})).toBeVisible();
  const shared=await page.evaluate(()=>JSON.parse(localStorage.getItem('daria.telemetry')||'[]').some((x:any)=>x.event==='result_share'&&x.method==='clipboard'));
  expect(shared).toBeTruthy();
});

test('mobile cold-load measurements stay within release budget',async({page,browserName},testInfo)=>{
  test.skip(browserName!=='chromium','CDP-style release metric is tracked on Chromium');
  await page.setViewportSize({width:390,height:844});
  const started=Date.now();
  const response=await page.goto('/?case=activation',{waitUntil:'load'});
  const wall=Date.now()-started;
  expect(response?.ok()).toBeTruthy();
  const metrics=await page.evaluate(()=>{
    const n=performance.getEntriesByType('navigation')[0] as PerformanceNavigationTiming;
    const resources=performance.getEntriesByType('resource') as PerformanceResourceTiming[];
    return {
      domContentLoadedMs:Math.round(n.domContentLoadedEventEnd),
      loadMs:Math.round(n.loadEventEnd),
      transferBytes:Math.round((n.transferSize||0)+resources.reduce((a,r)=>a+(r.transferSize||0),0)),
      encodedBytes:Math.round((n.encodedBodySize||0)+resources.reduce((a,r)=>a+(r.encodedBodySize||0),0)),
      domNodes:document.getElementsByTagName('*').length,
      visibleButtons:[...document.querySelectorAll('button')].filter((e:any)=>e.offsetParent!==null).length
    };
  });
  const fs=await import('node:fs');
  fs.mkdirSync('artifacts',{recursive:true});
  fs.writeFileSync('artifacts/mobile-performance-'+testInfo.project.name+'.json',JSON.stringify({...metrics,wallMs:wall,viewport:'390x844'},null,2));
  expect(metrics.encodedBytes).toBeLessThan(300000);
  expect(metrics.domNodes).toBeLessThan(500);
  expect(wall).toBeLessThan(2500);
});
