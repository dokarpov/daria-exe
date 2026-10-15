# DARIA.EXE — Product Office

DARIA.EXE is a product-judgement game prototype built around an explorable office rather than quiz cards.

## Current development baseline

The current browser code on `main` is **v4.1.0**, but the public build is offline while gameplay is being rebuilt and tested. It contains five rooms:

- **Research Lab** — observe users and collect behaviour signals.
- **Analytics** — trace a broken product flow.
- **Roadmap Room** — separate evidence-backed work from noise.
- **Experiment Bay** — isolate one variable before testing.
- **Launch Room** — return to the outcome after launch.

This build is a gameplay prototype. It is not evidence of learning impact, retention, or product-market fit.

## Development tracks

Experimental work is kept off `main`.

- `feature/product-office-tested` — browser UX/testing work.
- `unity/integration` — Unity integration base.
- `unity/research-lab` — Unity Research Lab development.
- `unity/research-lab-testing` — Unity gameplay/graphics test branch.

Unity work lives in `/unity` and does not replace the current browser release unless it passes its own validation gate.

## Release discipline

A branch is not a release because it renders or builds once. Promotion requires its own passing test suite and an explicit release decision.

Built by **Daria Karpova**.
