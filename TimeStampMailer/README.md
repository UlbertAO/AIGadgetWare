# TimeStampMailer

This project is an Azure Functions app that automates email delivery on a scheduled timer. It contains two timer-triggered functions that run every five minutes to send emails using different transport mechanisms: SMTP (via Gmail/other providers) or SendGrid API.

## What this project is about

It demonstrates how to build serverless background processes in C# that can automate email notifications. The functions read configurations from environment variables/settings and send emails when triggered.

## Functions Included

The application defines two separate timer-triggered Azure Functions:

### 1. `TimeStampMailer` (SMTP Mailer)
- **Class**: `TimeStampMailerFunction`
- **Purpose**: Sends test emails using a traditional SMTP server (e.g., Gmail).
- **Work/Behavior**:
  - Uses `MailKit` and `MimeKit` to connect to, authenticate with, and send messages through an SMTP server.
  - Reads settings (`SMTP_HOST`, `SMTP_PORT`, `SMTP_ADDRESS`, `SMTP_USER_NAME`, `SMTP_PASSWORD`, `RECIPIENT_EMAILS`) from environment variables.
  - If any configuration is missing, it logs an error and exits cleanly.

### 2. `SendGridMailerFunction` (SendGrid API Mailer)
- **Class**: `SendGridMailerFunction`
- **Purpose**: Sends test emails using the SendGrid API.
- **Work/Behavior**:
  - Uses the official `SendGrid` C# SDK to construct and send HTML email messages.
  - Reads configuration (`SENDGRID_API_KEY`, `SMTP_ADDRESS` as sender email, and `RECIPIENT_EMAILS` as recipient email) from environment variables.
  - Validates input strictly: raises an `InvalidOperationException` if any of these required configuration keys are missing or empty, failing the execution path clearly to prevent silent failures.

---

## Key points

- **Trigger:** Both functions are timer-triggered and run every 5 minutes (`0 */5 * * * *` with `RunOnStartup = true`).
- **Runtime:** Target framework is `net6.0` and Azure Functions v4 (in-process).
- **Storage:** Timer triggers require Azure Storage for schedule state tracking. Use Azurite (local emulator) or a real Azure Storage account.
- **Config:** `local.settings.json` holds runtime settings and credentials (do not commit secrets).

## Configuration (local)

1. Copy or edit `example.local.settings.json` into `local.settings.json` and set your values.
2. Configuration keys:
   - `AzureWebJobsStorage`: Storage connection string (Azurite or Azure Storage).
   - `FUNCTIONS_WORKER_RUNTIME`: `dotnet`
   - **SMTP Settings** (for `TimeStampMailer`):
     - `SMTP_HOST`: e.g. `smtp.gmail.com`
     - `SMTP_PORT`: e.g. `587`
     - `SMTP_ADDRESS`: Sender email address (e.g. `example@gmail.com`)
     - `SMTP_USER_NAME`: Display name of the sender
     - `SMTP_PASSWORD`: SMTP Password (use a Gmail App Password if using Gmail)
     - `RECIPIENT_EMAILS`: Comma-separated list of recipient emails
   - **SendGrid Settings** (for `SendGridMailerFunction`):
     - `SENDGRID_API_KEY`: Your SendGrid API Key (starts with `SG.`)
     - `SMTP_ADDRESS`: Used as the sender email address
     - `RECIPIENT_EMAILS`: The first email address in the list is used as the recipient

## Run locally (recommended)

1. Install prerequisites:
   - .NET 6 SDK
   - Azure Functions Core Tools
   - Node.js + npm (for Azurite)

2. Install and start Azurite (local storage emulator):

```bash
npm install -g azurite
azurite -s --location ./azurite_storage
```

3. Build and start the Functions host (from the `TimeStampMailer/TimeStampMailer` folder):

```bash
dotnet build
func start --verbose
```

The host will load both `TimeStampMailer` and `SendGridMailerFunction` functions and run them on the configured timer. Check logs for the send status of both.

## Result

![Received Mail](./TimeStampMailer/received_mail.png)
