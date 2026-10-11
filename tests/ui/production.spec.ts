import {test,expect} from '@playwright/test';

const u='/';

async function noOverflow(page:any){
  const x=await page.evaluate(()=>({w:innerWidth,sw:document.documentElement.scrollWidth,bw:document.body.scrollWidth}));
  expect(Math.max(x.sw,x.bw)).toBeLessThanOrEqual(x.w+1);
}

test('v4 is served',async({page})=>{
  const r=await page.goto(u);
  expect(r?.ok()).toBeTruthy();
  await expect(page.locator('meta[name="daria-build"]')).toHaveAttribute('content','4.0.0');
});

test('home exposes two playable modes',async({page})=>{
  await page.goto(u);
  await expect(page.getByRole('button',{name:/Quick incident/})).toBeVisible();
  await expect(page.getByRole('button',{name:/Investigate/})).toBeVisible();
});

test('quick play visibly develops over four moves',async({page})=>{
  await page.goto(u);
  await page.getByRole('button',{name:/Quick incident/}).click();
  await expect(page.getByText('Move 1 of 4')).toBeVisible();
  const before=await page.locator('#qHealth').getAttribute('style');
  await page.locator('[data-a="1"]').click();
  await page.waitForTimeout(2050);
  await expect(page.getByText('Move 2 of 4')).toBeVisible();
  const after=await page.locator('#qHealth').getAttribute('style');
  expect(after).not.toBe(before);
});

test('quick play reaches an ending',async({page})=>{
  await page.goto(u);
  await page.getByRole('button',{name:/Quick incident/}).click();
  for(let i=0;i<4;i++){
    await page.locator('[data-a="1"]').click();
    await page.waitForTimeout(2050);
  }
  await expect(page.locator('#final')).toHaveClass(/active/);
  await expect(page.locator('#finalTitle')).not.toHaveText('');
});

test('investigate controls are clickable and route evidence',async({page})=>{
  await page.goto(u);
  await page.getByRole('button',{name:/Investigate/}).click();
  await expect(page.getByRole('button',{name:/Scan next source/})).toBeEnabled();
  await page.getByRole('button',{name:/Scan next source/}).click();
  await expect(page.locator('#evidenceBubble')).toHaveClass(/show/);
  await page.getByRole('button',{name:'traffic mix'}).click();
  await expect(page.locator('#iScore')).toHaveText('1');
  await expect(page.locator('#evidenceBubble')).not.toHaveClass(/show/);
});

test('investigation develops hypotheses and allows a call',async({page})=>{
  await page.goto(u);
  await page.getByRole('button',{name:/Investigate/}).click();
  await page.getByRole('button',{name:/Scan next source/}).click();
  await page.getByRole('button',{name:'traffic mix'}).click();
  await page.getByRole('button',{name:/Scan next source/}).click();
  await page.getByRole('button',{name:'pricing friction'}).click();
  await expect(page.getByRole('button',{name:/Make the call/})).toBeEnabled();
  await page.getByRole('button',{name:/Make the call/}).click();
  await expect(page.locator('#final')).toHaveClass(/active/);
});

for(const [name,w,h] of [['320',320,700],['390',390,844],['landscape',844,390]] as const){
  test(name+' no horizontal overflow',async({page})=>{
    await page.setViewportSize({width:w,height:h});
    await page.goto(u);await noOverflow(page);
    await page.getByRole('button',{name:/Investigate/}).click();await noOverflow(page);
  });
}

test('keyboard focus is visible',async({page})=>{
  await page.goto(u);
  await page.keyboard.press('Tab');
  const ok=await page.evaluate(()=>{
    const e=document.activeElement as HTMLElement;
    return e?.tagName==='BUTTON'&&getComputedStyle(e).outlineStyle!=='none';
  });
  expect(ok).toBeTruthy();
});
