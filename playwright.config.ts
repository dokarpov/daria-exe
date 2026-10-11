import {defineConfig,devices} from '@playwright/test';
export default defineConfig({
  testDir:'./tests/ui', timeout:30000, expect:{timeout:7000}, fullyParallel:false, retries:1,
  reporter:[['list'],['html',{outputFolder:'artifacts/playwright-report',open:'never'}]],
  use:{baseURL:'http://127.0.0.1:4173',trace:'retain-on-failure',screenshot:'only-on-failure',video:'retain-on-failure'},
  webServer:{command:'npm run preview',url:'http://127.0.0.1:4173',reuseExistingServer:true,timeout:120000},
  projects:[
    {name:'desktop-chromium',use:{...devices['Desktop Chrome'],viewport:{width:1440,height:900}}},
    {name:'desktop-webkit',use:{...devices['Desktop Safari'],viewport:{width:1280,height:800}}},
    {name:'iphone-webkit',use:{...devices['iPhone 15 Pro']}},
    {name:'android-chromium',use:{...devices['Pixel 7']}},
    {name:'ipad-webkit',use:{...devices['iPad Pro 11']}}
  ]
});
