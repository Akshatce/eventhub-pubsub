


# Deploying Azure Resources with the Provided Bicep File

This guide helps you deploy an Azure Event Hub, Event Hub Namespace, and a Storage Account using the provided Bicep file.


## Overview of the Bicep File

The Bicep file defines:
- An **Event Hub Namespace**.
- An **Event Hub** with a retention policy and partitions.
- A **Storage Account** for checkpointing or other storage purposes.

The deployment parameters allow flexibility to specify the project name, location, SKU tiers, and other configurations.

---

## Prerequisites

Before deploying the Bicep file, ensure you have:
1. **Azure CLI installed**: [Download Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
2. Logged into your Azure account using `az login`.
3. Access to an Azure subscription and resource group where the resources will be deployed.

### Create a Resource Group (if not already created)

```bash
az group create --name <resource-group-name> --location <location-name>
```

**Example**:  
```bash
az group create --name aeh-rg --location eastus
```

---

## Bicep File Parameters

| **Parameter**    | **Description**                                               | **Default Value**       |
|-------------------|---------------------------------------------------------------|-------------------------|
| `projectName`     | Specifies a project name used to generate Event Hub and Namespace names. | Required input          |
| `location`        | Azure location where resources will be deployed.             | Resource group’s location |
| `eventHubSku`     | Specifies the messaging tier for Event Hub Namespace.         | `Standard`             |
| `storageskuName`  | The SKU for the Storage Account.                              | `Standard_LRS`         |
| `kind`            | The kind of the Storage Account.                              | `StorageV2`            |

---

## Steps to Deploy the Bicep File

### 1. Save the Bicep File

Save the provided Bicep file content to a file named `main.bicep` on your local machine.

```bash
touch main.bicep
# Copy and paste the provided Bicep file content into `main.bicep`
```

---

### 2. Verify the File Syntax

Run the following command to ensure the Bicep file syntax is correct:

```bash
az bicep build --file main.bicep
```

This will check for syntax errors and build an ARM JSON equivalent if necessary.

---

### 3. Deploy the Bicep File

Run the deployment command:

```bash
az deployment group create \
  --resource-group <your-resource-group-name> \
  --template-file main.bicep \
  --parameters projectName=<your-project-name> location=<your-location> eventHubSku=<EventHubType> storageskuName=<storage_type> kind=<storage_kind>
```

**Replace**:
- `<your-resource-group-name>`: The name of your Azure resource group.
- `<your-project-name>`: A custom project name.
- `<your-location>`: The desired Azure region (e.g., `eastus`).

---

## Understanding the Resources

### 1. **Event Hub Namespace**
- **Name**: Dynamically generated as `<projectName>ns`.
- **Tier**: Configurable as `Basic` or `Standard`.
- **Purpose**: Provides a container for Event Hubs with capacity scaling options.

### 2. **Event Hub**
- **Name**: Dynamically generated from the project name.
- **Retention**: Retains messages for 7 days.
- **Partitions**: Configured with 2 partitions for concurrent processing.

### 3. **Storage Account**
- **Name**: Dynamically generated as `<projectName>-storage`.
- **SKU**: Configurable as `Standard_LRS` or other options.
- **Kind**: Defined as `StorageV2` with hot access tier.

---

## Validating the Deployment

After deployment:
1. Go to the **Azure Portal**.
2. Navigate to your resource group to view the deployed resources:
   - Verify the **Event Hub Namespace** and **Event Hub**.
   - Check the **Storage Account** configurations.

---

## Cleanup

If you want to remove the resources after testing, delete the resource group:

```bash
az group delete --name <your-resource-group-name> --yes --no-wait
```

---

## Troubleshooting

### Deployment Failed?
- Verify the parameter values (e.g., ensure the project name is alphanumeric).
- Ensure you have correct permissions on the subscription and resource group.
- Check the Azure CLI output for detailed error messages.

---

## Need More Information?

Refer to the [official Azure Bicep documentation](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/).
