
# MOP.Core

Contains _Models_ and _Interfaces_ to be shared between _Hosts_, _Clients_ and _Plugins_

## Akka

Contains models and factories specific for AKKA related logic

### Hocon

**_AkkaBaseConfig.hocon_**: base HOCON file to use when starting akka, some properties 
can be changes using `HoconConfigFactory.cs`, such props are in the format `#!NAME`

**_HoconConfig_**: Model of properties to be replaced in `AkkaBaseConfig.hocon` file

**_HoconConfigFactory_**: Factory to build an HOCON file based on a source input
and an object that has properties with the attribute `HoconPropertyAttribute`

**_HoconFileLoader_**: Helper to load the `AkkaBaseConfig.hocon` file from the assembly resources

**_HoconPropertyAttribute_**: Property attribute to mark a property to be replaced in an HOCON file

---

## Domain

MOP domain specif sources

### Api

Contains logic related to core and plugins api reference, so remote clients know how to execute actions in a host

**_ActorAction_**: Model of a action that can be called by clients

**_ActorActionAttribute_**: Method attribute to allow it to be called by clients

**_ActorActionFactory_**: Factory to build an `ActorAction` from an `MethodInfo`

**_ActorInstance_**: Group of `ActorActions` with a call path

**_ActorInstanceAttribute_**: Class attribute to allow its actions to be grouped

**_ActorInstanceFactory_**: Factory to build an `ActorInstance` from an `Type`

**_ArgumentItem_**: Model of arguments for a `ActorAction`

**_CommandInvoke_**: Command to be sent by clients in order to invoke a action

### Events

Contains logic related to Events commands

**_Event_**: Implements `IEvent`

**_EventCommand_**: Command to be sent by clients, generates and event in a Host

**_EventsExtensions_**: Extension helpers for `IEvent`

**_IEvent_**: Model of an Host event

**_ReplayCommand_**: Command to replay all events from a specified moment

**_SubscribeCommand_**: Allow clients to subscribe to host events

**_Unit_**: Empty object to use on event commands without body

### Host

Host interfaces

**_HostInfo_**: implements `IHostInfo`

**_IHost_**: Host interface

**_IHostInfo_**: Interface for host specific information

### Plugins

**_BaseActorPlugin_**: Base implementation for plugins based on actors

**_BasePlugin_**: Base implementation for IPlugin

**_InjectableAttribute_**: Mark class to be injected at plugin load time

**_IPlugin_**: Interface for all plugins

**_IPluginInfo_**: Information for a plugin

**_LifeCycle_**: Enum for life cycle of a plugin service

**_PluginInfo_**: default implementation for IPluginInfo

**_PluginPriority_**: standard values to use as plugin priority

---

## Helpers

Static classes with helper methods and extensions

**_InfoBuilder_**: Helpers for IPluginInfo

**_MopHelper_**: Helpers for MOP

---

## Infra

Infrastructure related objects

**_MopLifeService_**: Singleton service to await for application to exit

**_MopVersion_**: Represents a SemVersion instance, and allow to compare it

### Collections

**_IncrementalCollection_**: Collection of values, where each entry has an automatic created ID

### Extensions

Contains extensions for multiple purposes

### Optional

`Option<T>` related methods

### Tools

Tool methods that doesn't fit any specific category

---

## Services

Default services interfaces

**_IApiService_**: Manage and read api documentation

**_IConfigService_**: Allow to save to disk and read from disk, serializable objects

**_IEventsService_**: Emit and subscribe events

**_IInjectorService_**: IoC for MOP

**_ILogService_**: Create log entries

**_IPluginService_**: Manage plugins

---

## UserSettings

Save and load initial settings for current user

**_IUserSettingsLoader_**: Interface for user settings loaders

**_UserSettingsFactory_**: Factory to create default settings loader

**_UserSettingsLoader_**: Default `IUserSettingsLoader` implementation
