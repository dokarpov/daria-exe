import {test,expect} from '@playwright/test';

test('scenario catalogue and runtime are separate, served assets',async({page,request})=>{
  for(const path of ['/cases.js','/game.js']){
    const response=await request.get(path);
    expect(response.ok(),path).toBeTruthy();
  }
  const errors:string[]=[];
  page.on('pageerror',error=>errors.push(error.message));
  await page.goto('/?case=career_offer');
  await expect(page.getByRole('button',{name:/Enter case/})).toBeVisible();
  await expect(page.locator('#homeCaseTitle')).not.toBeEmpty();
  expect(errors).toEqual([]);
});

test('runtime still supports an initial action and meaningful response',async({page})=>{
  await page.goto('/?case=career_offer');
  await page.getByRole('button',{name:/Enter case/}).click();
  await expect(page.locator('.actionCard')).toHaveCount(4);
  await page.locator('.actionCard').filter({hasText:'Ask'}).click();
  await expect(page.getByText(/NEW INFORMATION/)).toBeVisible();
});
