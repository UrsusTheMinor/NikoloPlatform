#!/bin/bash

# Ask for the migration name
echo "Enter migration name:"
read migration_name

# Check if a name was provided
if [ -z "$migration_name" ]; then
    echo "Migration name cannot be empty!"
    exit 1
fi

# Add the migration
dotnet ef migrations add "$migration_name" --project Nikolo.Data --startup-project Nikolo.Api

# Check if the migration was successful
if [ $? -eq 0 ]; then
    echo "Migration created successfully. Updating database..."
    dotnet ef database update --project Nikolo.Data --startup-project Nikolo.Api
    if [ $? -eq 0 ]; then
        echo "Database updated successfully."
    else
        echo "Database update failed!"
        exit 1
    fi
else
    echo "Migration creation failed!"
    exit 1
fi

