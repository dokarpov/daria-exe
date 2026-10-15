import {test,expect} from '@playwright/test';

const u='/v4.html';

async function noOverflow(page:any){
  const x=await page.evaluate(()=>({w:innerWidth,sw:document.documentElement.scrollWidth,bw:document.body.scrollWidth}));
  expect(Math.max(x.sw,x.bw)).toBeLessThanOrEqual(x.w+1);
}

test('v4 starts with one explicit objective and controls',async({page})=>{
  await page.goto(u);
  await expect(page.getByRole('heading',{name:/Your first day is already on fire/i})).toBeVisible();
  await expect(page.getByText(/Start in the Research Lab/i)).toBeVisible();
  await expect(page.locator('#intro').getByText(/WASD|arrow keys/i)).toBeVisible();
  await expect(page.locator('#intro').getByText(/E or Space/i)).toBeVisible();
  await expect(page.getByRole('button',{name:/Enter office/i})).toBeVisible();
});

test('after entering, controls and next action remain visible',async({page})=>{
  await page.goto(u);
  await page.getByRole('button',{name:/Enter office/i}).click();
  await expect(page.locator('#controlsHint')).toBeVisible();
  await expect(page.locator('#nextStep')).toBeVisible();
  await expect(page.locator('#nextStep')).toContainText(/Research Lab/i);
  await expect(page.locator('#mission')).toContainText(/Research Lab/i);
});

test('keyboard movement changes the player position',async({page})=>{
  await page.goto(u);
  await page.getByRole('button',{name:/Enter office/i}).click();
  const before=await page.locator('#playerState').getAttribute('data-x');
  await page.keyboard.down('ArrowLeft');
  await page.waitForTimeout(500);
  await page.keyboard.up('ArrowLeft');
  const after=await page.locator('#playerState').getAttribute('data-x');
  expect(after).not.toBe(before);
});

test('Research Lab explains the physical action, not just the goal',async({page})=>{
  await page.goto(u);
  await page.getByRole('button',{name:/Enter office/i}).click();
  await page.evaluate(()=>{
    const api=(window as any).__DARIA_TEST__;
    api.movePlayer(210,180);
  });
  await expect(page.locator('#chapter')).toHaveText(/Research Lab/i);
  await expect(page.locator('#hint')).toContainText(/follow/i);
  await expect(page.locator('#hint')).toContainText(/walk into/i);
  await expect(page.locator('#nextStep')).toContainText(/signal/i);
});

test('Research completion produces a clear next step',async({page})=>{
  await page.goto(u);
  await page.getByRole('button',{name:/Enter office/i}).click();
  await page.evaluate(()=>{
    const api=(window as any).__DARIA_TEST__;
    api.completeResearch();
  });
  await expect(page.locator('#score')).toHaveText('1');
  await expect(page.locator('#nextStep')).toContainText(/Analytics/i);
});

for(const [name,w,h] of [['320',320,700],['390',390,844],['landscape',844,390]] as const){
  test(name+' has no horizontal overflow',async({page})=>{
    await page.setViewportSize({width:w,height:h});
    await page.goto(u);
    await noOverflow(page);
    await page.getByRole('button',{name:/Enter office/i}).click();
    await noOverflow(page);
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
