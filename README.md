# Minimal Fable Demo using React

The master branch is the finished version with Preact and the aliases in the webpack config set.
This branch is the original, before using Preact.
To run locally, simply issue the following commands in the root after cloning

`npm install`

`npm start`

---

To build a production bundle, run `npm run build` and then use something like [dotnet serve](https://github.com/natemcmaster/dotnet-serve) to serve the bundle from the `deploy` directory

---

It uses Fable 2.1, which doesn't require a separate fable deamon to be started via dotnet fable anymore. Instead webpack starts a fable compiler to compile F# to JS.
