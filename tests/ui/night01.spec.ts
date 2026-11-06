// rerun after Pages deploy
import {test,expect} from '@playwright/test';

const live='https://dokarpov.github.io/daria-exe/preview/night01/';

async function openLoaded(page){
  const errors:string[]=[];
  page.on('pageerror',e=>errors.push('pageerror: '+e.message));
  page.on('response',r=>{if(r.status()>=400&&!/favicon|apple-touch-icon/i.test(r.url()))errors.push('http '+r.status()+': '+r.url())});
  page.on('console',m=>{if(m.type()==='error'&&!m.text().includes('Failed to load resource'))errors.push('console: '+m.text())});
  const response=await page.goto(live,{waitUntil:'domcontentloaded',timeout:30000});
  expect(response?.ok()).toBeTruthy();
  await expect(page.locator('#boot')).toHaveClass(/done/,{timeout:5000});
  await expect(page.locator('#boot')).toBeHidden({timeout:6000});
  expect(errors).toEqual([]);
  return errors;
}

async function playCorePath(page){
  await page.locator('#enterGame').scrollIntoViewIfNeeded();
  await page.locator('#enterGame').click();
  await expect(page.locator('#game')).toHaveClass(/active/);
  await expect(page.locator('#sceneName')).toHaveText('LOBBY');

  await page.getByRole('button',{name:'Analytics'}).click();
  await expect(page.locator('#sceneName')).toHaveText('ANALYTICS');
  await page.getByRole('button',{name:/Inspect activation funnel/i}).click();
  await expect(page.locator('#line')).toContainText('contradiction');

  await page.getByRole('button',{name:/Go to Research/i}).click();
  await expect(page.locator('#sceneName')).toHaveText('RESEARCH LAB');
  await page.getByRole('button',{name:/Observe session/i}).click();
  await expect(page.locator('#line')).toContainText('stuck after it');

  await page.getByRole('button',{name:/Make the call/i}).click();
  await expect(page.locator('#sceneName')).toHaveText('LAUNCH CONTROL');
  await page.getByRole('button',{name:/Commit decision/i}).click();
  await expect(page.locator('#choiceLayer')).toHaveClass(/active/);
  await expect(page.locator('.decision')).toHaveCount(3);
  await page.locator('.decision').filter({hasText:'Hold'}).click();
  await expect(page.locator('#sceneName')).toHaveText('ROOFTOP');
  await expect(page.locator('#line')).toContainText('rollout pauses');
}

test('Night 01 loads, tells the story and completes the core branch',async({page},testInfo)=>{
  const errors=await openLoaded(page);

  const overflow=await page.evaluate(()=>document.documentElement.scrollWidth>innerWidth+1);
  expect(overflow).toBeFalsy();

  if(['desktop-chromium','iphone-webkit'].includes(testInfo.project.name)){
    await page.screenshot({path:`qa/night01/${testInfo.project.name}-landing.png`,fullPage:false});
  }

  await page.locator('#enterGame').scrollIntoViewIfNeeded();
  if(['desktop-chromium','iphone-webkit'].includes(testInfo.project.name)){
    await page.screenshot({path:`qa/night01/${testInfo.project.name}-entry.png`,fullPage:false});
  }

  await page.locator('#enterGame').click();
  await expect(page.locator('#game')).toHaveClass(/active/);
  await expect(page.locator('#sceneName')).toHaveText('LOBBY');
  if(['desktop-chromium','iphone-webkit'].includes(testInfo.project.name)){
    await page.screenshot({path:`qa/night01/${testInfo.project.name}-lobby.png`,fullPage:false});
  }

  await page.getByRole('button',{name:'Analytics'}).click();
  await page.getByRole('button',{name:/Inspect activation funnel/i}).click();
  await page.getByRole('button',{name:/Go to Research/i}).click();
  await page.getByRole('button',{name:/Observe session/i}).click();
  await page.getByRole('button',{name:/Make the call/i}).click();
  await page.getByRole('button',{name:/Commit decision/i}).click();
  await page.locator('.decision').filter({hasText:'Hold'}).click();
  await expect(page.locator('#sceneName')).toHaveText('ROOFTOP');
  await expect(page.locator('#line')).toContainText('rollout pauses');

  if(['desktop-chromium','iphone-webkit'].includes(testInfo.project.name)){
    await page.screenshot({path:`qa/night01/${testInfo.project.name}-ending.png`,fullPage:false});
  }

  const events=await page.evaluate(()=>JSON.parse(localStorage.getItem('daria.v4.events')||'[]').map((x:any)=>x.event));
  expect(events).toContain('mission_start');
  expect(events).toContain('clue_found');
  expect(events).toContain('decision');
  expect(events).toContain('mission_complete');
  expect(errors).toEqual([]);

  const finalOverflow=await page.evaluate(()=>document.documentElement.scrollWidth>innerWidth+1);
  expect(finalOverflow).toBeFalsy();
});

test('three decision branches resolve to distinct consequences',async({page})=>{
  for(const choice of ['Scale','Roll it back','Hold']){
    await openLoaded(page);
    await page.locator('#enterGame').scrollIntoViewIfNeeded();
    await page.locator('#enterGame').click();
    await page.getByRole('button',{name:'Analytics'}).click();
    await page.getByRole('button',{name:/Inspect activation funnel/i}).click();
    await page.getByRole('button',{name:/Make the call/i}).click();
    await page.getByRole('button',{name:/Commit decision/i}).click();
    await page.locator('.decision').filter({hasText:choice}).click();
    await expect(page.locator('#sceneName')).toHaveText('ROOFTOP');
    await expect(page.locator('#line')).not.toBeEmpty();
  }
});

test('mobile controls remain tappable and loader has a hard recovery path',async({page},testInfo)=>{
  test.skip(!['iphone-webkit','android-chromium'].includes(testInfo.project.name));
  await openLoaded(page);
  const min=await page.evaluate(()=>Math.min(...[...document.querySelectorAll('button')].filter((b:any)=>b.offsetParent!==null).map((b:any)=>Math.min(b.getBoundingClientRect().width,b.getBoundingClientRect().height))));
  expect(min).toBeGreaterThanOrEqual(44);
  await page.locator('#enterGame').scrollIntoViewIfNeeded();
  await page.locator('#enterGame').click();
  await expect(page.locator('#game')).toHaveClass(/active/);
  const gameOverflow=await page.evaluate(()=>document.documentElement.scrollWidth>innerWidth+1);
  expect(gameOverflow).toBeFalsy();
});

test('reduced motion disables scene and loader animations',async({page})=>{
  await page.emulateMedia({reducedMotion:'reduce'});
  await openLoaded(page);
  const values=await page.evaluate(()=>({
    enter:getComputedStyle(document.querySelector('.enter')!).transitionDuration,
    intro:getComputedStyle(document.querySelector('.introScene')!).transitionDuration,
  }));
  expect(values.enter).toMatch(/^0s/);
  expect(values.intro).toMatch(/^0s/);
});
