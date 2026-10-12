import {test,expect} from '@playwright/test';

const u='/';

async function noOverflow(page:any){
  const x=await page.evaluate(()=>({w:innerWidth,sw:document.documentElement.scrollWidth,bw:document.body.scrollWidth}));
  expect(Math.max(x.sw,x.bw)).toBeLessThanOrEqual(x.w+1);
}

test('Product Office opens with one clear first action',async({page})=>{
  await page.goto(u);
  await expect(page.getByRole('heading',{name:/Your first day is already on fire/i})).toBeVisible();
  await expect(page.getByRole('button',{name:/Enter office/i})).toBeVisible();
  await expect(page.getByRole('button',{name:/Controls/i})).toBeVisible();
  await expect(page.getByText(/Research Lab/i)).toBeVisible();
});

test('controls are explained before gameplay',async({page})=>{
  await page.goto(u);
  await page.getByRole('button',{name:/Controls/i}).click();
  await expect(page.getByText(/WASD.*arrows/i)).toBeVisible();
  await expect(page.getByText(/E.*Space.*interact/i)).toBeVisible();
});

test('gameplay keeps controls and next objective visible',async({page})=>{
  await page.goto(u);
  await page.getByRole('button',{name:/Enter office/i}).click();
  await expect(page.locator('#controlsHint')).toBeVisible();
  await expect(page.locator('#controlsHint')).toContainText(/WASD|arrows/i);
  await expect(page.locator('#nextStep')).toBeVisible();
  await expect(page.locator('#nextStep')).toContainText(/Research Lab/i);
  await expect(page.locator('#mission')).not.toHaveText('');
  await expect(page.locator('#hint')).not.toHaveText('');
});

test('keyboard movement visibly changes player state',async({page})=>{
  await page.goto(u);
  await page.getByRole('button',{name:/Enter office/i}).click();
  const before=await page.locator('#playerState').getAttribute('data-x');
  await page.keyboard.down('ArrowLeft');
  await page.waitForTimeout(500);
  await page.keyboard.up('ArrowLeft');
  const after=await page.locator('#playerState').getAttribute('data-x');
  expect(after).not.toBe(before);
});

for(const [name,w,h] of [['320',320,700],['390',390,844],['landscape',844,390]] as const){
  test(name+' no horizontal overflow',async({page})=>{
    await page.setViewportSize({width:w,height:h});
    await page.goto(u);
    await noOverflow(page);
    await page.getByRole('button',{name:/Enter office/i}).click();
    await noOverflow(page);
  });
}

test('focus is visible for keyboard players',async({page})=>{
  await page.goto(u);
  await page.keyboard.press('Tab');
  const ok=await page.evaluate(()=>{
    const e=document.activeElement as HTMLElement;
    return e?.tagName==='BUTTON'&&getComputedStyle(e).outlineStyle!=='none';
  });
  expect(ok).toBeTruthy();
});
