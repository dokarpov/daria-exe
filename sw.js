// Offline shell is scoped to the project path (e.g. /daria-exe/ on Pages).
const VERSION='daria-v3.8.0-split-20261106';
const scope=self.registration.scope;
const entry=new URL('index.html',scope).href;
const assets=['', 'index.html', 'cases.js', 'game.js'].map(name=>new URL(name,scope).href);
const scripts=new Set(['cases.js','game.js'].map(name=>new URL(name,scope).href));

self.addEventListener('install',event=>{
  event.waitUntil(caches.open(VERSION).then(cache=>cache.addAll(assets))
    .then(()=>self.skipWaiting()));
});
self.addEventListener('activate',event=>{
  event.waitUntil(caches.keys()
    .then(keys=>Promise.all(keys.filter(key=>key.startsWith('daria-')&&key!==VERSION)
      .map(key=>caches.delete(key))))
    .then(()=>self.clients.claim()));
});
self.addEventListener('fetch',event=>{
  const req=event.request;
  if(req.method!=='GET'||new URL(req.url).origin!==new URL(scope).origin)return;
  if(req.mode==='navigate'){
    event.respondWith(fetch(req).catch(()=>caches.match(entry)));
  } else if(scripts.has(req.url)){
    event.respondWith(fetch(req).catch(()=>caches.match(req)));
  }
});
