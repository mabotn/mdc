# Mabo.Data.Collections

A NoSQL Documents in local storage database service designed for Universal Windows Platform apps, querying through Linq.

When using MDC (Mabo.Data.Collections) make sure you add this line

```
using Mabo.Data.Collections;
```

## Creating a collection

Instead of using table, with MDC we’ve got collections
```
Collection tasks = new Collection("tasks");
```
## Working with a collection

### Creating a collection item

All item models should derive from CollectionItem type

```
public class Task : CollectionItem {
	public string Details { get; set; }
	public bool Done { get; set; }
}
```
### Inserting a collection
```
Task t = new Task() {
	Details = "Fork Me",
	Done = false
};

await tasks.InsertAsync<Task>(t);
```
### Reading items from collection
```
List<Task> tasksList = tasks.SelectAsync<Task>();
```	
### Updating an item
```
t.Done = true;
tasks.UpdateAsync<Task>(t);
```
### Deleting an item
```
tasks.DeleteAsync<Task>(t);
```

## Dependencies

Before using MDC, make sure you've [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json) installed

## Contributors

MDC project is initially developed by [Mabo](http://www.mabo.tn/)
