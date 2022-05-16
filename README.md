# News
## Fix Release error in this feature on windows os, fail in mkdir command fixed
This version we introduce the view models grouped in a folder, this folder is named with the name of model. 
Commands affected:

```bash
cm g model --with-script <Model Name>
cm g model --with-all-scripts --forced
cm g model --with-all-scripts --safety 
```

# Documentation

Create New Web Api: 
```bash
cm new <APP_NAME>
```

Create Model (And ViewModels) Templeate: 
```bash
cm (g|generate) model <MODEL_NAME>
````

Create Model from json file script:
```bash
 cm (g|generate) model --with-script <MODEL_JSON_FILE_NAME> 
```
Warning: Json`s file script are in Entities/JsonModelsDefinition folder.

Create Model from all json files overwritting generated files: 
```bash
cm (g|generate) model --with-all-scripts --safety 
```
Warning: Json`s file script are in Entities/JsonModelsDefinition folder.

Create Model from all json files without overwritting generated files: 
```bash
cm (g|generate) model --with-all-scripts --force
```
 Warning: Json`s file script are in Entities/JsonModelsDefinition folder.

Generate Repository based on Model: 
```bash
cm (g|generate) repository <MODEL_NAME>
```

Generate CRUD Service based on Model: 
```bash
cm (g|generate) service-crud --model <MODEL_NAME>
````

Generate CRUD Controller based on Model:
```bash
 cm (g|generate) controller-crud --model <MODEL_NAME>
```

Update Repository Extension to dependency injection mapping: 
```bash
cm repository-di
```

Update Service Extension to dependency injection mapping: 
```bash
cm service-di
```

Add Packge in current project: 
```bash
cm add <PACKAGE_NAME> or cm add <PACKAGE_NAME> --verison <VERSION_NUMBER>
```

## Entity Framework Commands 
Warning: require dotnet-ef installed and execute on Api folder📁.

Add Migration:
```bash
cm ef add-migration <MIGRATION_NAME>
```

Remove Migration: 
```bash
cm ef remove-migration
```

List Migration: 
```bash
cm ef list-migration
```

List Migration: 
```bash
cm ef update-database
```
