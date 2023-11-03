# NetForge - Grouping

## Create Groups for Entities

Before assigning entities to specific groups, users need to define the groups to which the entities will belong.
To create a new group, utilize the Fluent API. Each group must have a unique name, and a description is optional.

```csharp
.AddGroup("Test Name", "Test Description")
.AddGroup("Test Name 1", "Test Description 1")
.AddGroup("Test Empty");
```

## Configuration

By default, entities are assigned to the "empty" group. Grouping can be customized either through the Fluent API or by using attributes.
When assigning entities to a group, users only need to specify the group's name.

### Fluent API

```csharp
.ConfigureEntity<Shop>(entityOptionsBuilder =>
{
    entityOptionsBuilder.SetGroup("Test Name");
})
```

### Attribute

```csharp
[NetForgeEntity(GroupName = "Test Name")]
public class Address
```