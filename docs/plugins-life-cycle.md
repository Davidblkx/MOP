# Plugins

## About

## How to install

## Lifecycle

When a plugin is loaded it go through 2 main phases

### Preload

During preload the Host is injected into the plugins. The priority is not taken into account during this, so order and availability of other plugins is not guarantied. This phase should be used to initialize variables and resources required in 2nd phase

### Initialize

During this phase the plugin should start all is processes