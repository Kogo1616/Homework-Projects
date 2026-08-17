# Deploying PharmaPrice to Azure (GitHub Actions → App Service)

This app is one deployable unit: `dotnet publish backend/backend.csproj` builds the
React frontend and bundles it into the backend's `wwwroot`, so a single App Service
serves both the API (`/api/*`) and the UI.

- **Host:** Azure App Service `pharmaprice` (Linux, .NET 10) — already created.
- **CI/CD:** [`.github/workflows/deploy.yml`](.github/workflows/deploy.yml) — builds on
  a GitHub-hosted Ubuntu runner and deploys to the App Service on every push to `main`.

> An `azure-pipelines.yml` is also included if you ever switch to Azure DevOps instead.

---

## Setup (one time)

### 1. Enable basic-auth publishing on the App Service
The publish-profile deploy method needs SCM basic auth, which is currently **off**.
Azure Portal → your `pharmaprice` App Service → **Settings → Configuration →
Platform settings** (or **Settings → Basic authentication**) → set
**SCM Basic Auth Publishing Credentials = On** → **Save**.

### 2. Set the Linux startup command (safety)
App Service → **Settings → Configuration → General settings** →
**Startup Command:** `dotnet backend.dll` → **Save**.

### 3. Download the publish profile
App Service → **Overview** → top toolbar **⬇ Download publish profile**. This gives a
`.PublishSettings` XML file. Open it and copy its **entire contents**.

### 4. Add it as a GitHub secret
In your GitHub repo → **Settings → Secrets and variables → Actions → New repository
secret**:
- **Name:** `AZURE_WEBAPP_PUBLISH_PROFILE`
- **Value:** paste the full contents from step 3

### 5. Push this code to GitHub
From this folder:
```bash
git init
git add .
git commit -m "Initial commit"
git branch -M main
git remote add origin https://github.com/<your-username>/PharmaPrice.git
git push -u origin main
```

That push triggers the workflow. Watch it under the repo's **Actions** tab.

---

## Result
Once the workflow's green, the app is live at your App Service URL
(`https://pharmaprice-<suffix>.<region>.azurewebsites.net` — find the exact URL on the
App Service **Overview** page).

Every future push to `main` redeploys automatically.

---

## Notes
- **App name must match:** `AZURE_WEBAPP_NAME` in the workflow = `pharmaprice`. If your
  App Service has a different name, edit that line.
- **No CORS needed in prod:** frontend and API share one origin; the dev-only CORS policy
  (`localhost:5173`) is irrelevant in production.
- **F1 tier:** the app sleeps when idle → first request after a pause is slow (cold start).
  Scale to B1 in the portal (one click, no redeploy) if that bothers you.
- **Legal reminder** (from the README): data comes from pharmacies' public web endpoints
  ("grey area"). For a serious long-term product, pursue official partnerships/APIs.
