# Support Ticket Dashboard — Trainee Technical Test
## The Scenario

You're stepping into a project left behind by a junior developer who was
abruptly moved to another team. This dashboard is used by IT to view,
filter, and prioritize support tickets. The UI is incomplete, the buttons
don't work, and the core backend logic for organizing tickets is broken.

Your job: follow the `// TODO:` comments across the project files to fix
the backend logic, wire up the frontend controls, and complete the styling.

## The Goal

This is what the finished dashboard looks like and does. Use these as your
reference — you're rebuilding this behavior.

**Default view — all 8 tickets, laid out in a responsive grid:**

![Default view](screenshots/01-default-view.png)

**"Show Critical" — filtered to only Critical-priority tickets, button highlighted active:**

![Critical filter](screenshots/02-critical-filter.png)

**"Show Open" — filtered to only Open tickets:**

![Open filter](screenshots/03-open-filter.png)

## Rules

- **Time limit: 2.5 hours**, starting once you've finished Phase 0 below
  (repo cloned, your branch created, `dotnet run` confirmed working). Getting
  set up doesn't count against your time. Roughly 90 minutes backend, 60
  minutes frontend from there — the backend is the harder, more
  heavily-weighted half of this exercise.
- **No internet access, except GitHub for this repository.** You may fork,
  clone, and push to your own fork of the assessment repo, and open a Pull
  Request against it (see Phase 0) — nothing else online. No searching, no
  documentation sites, no AI tools, no browsing other repos.
- **No installs.** Don't run `npm install`, `dotnet add package`, or add any
  dependencies — you're only editing the files that already exist. (Running
  `tools/setup-dotnet.ps1`/`.sh` in Phase 0 doesn't count as an install —
  it only unzips a folder and points `PATH` at it for your terminal
  session; it changes nothing system-wide.)
- You have VS Code and nothing else. That's all you need.
- Search the project for `TODO` to find every place you need to make a change.
- Not every bug in this project is marked with a `TODO` — see Phase 1 below.
- **This repo is public, and so is your Pull Request the moment you open
  it.** Anyone — including other candidates — can technically view any
  open PR against this repo. You may not view, read, or copy any Pull
  Request other than your own until a proctor tells you the assessment
  period has fully ended. Doing so, or being found to have done so,
  disqualifies your submission.

## Phase 0 — Get Set Up (untimed, do this first)

Do this before starting your 2.5-hour timer. None of it is being scored —
it just gets you to a working starting point.

1. Open a terminal (in VS Code: **Terminal → New Terminal**).

2. Check git is installed:
   ```
   git --version
   ```
   If that errors instead of printing a version number, ask a proctor
   before continuing.

3. **Fork the repository.** In a browser, go to
   `https://github.com/SkyeSenatla/SupportTicketDashboard_StarterPack`,
   sign in to your own GitHub account (create a free one now if you don't
   have one), and click **Fork** (top-right) to create your own copy of it
   under your account. You'll push your work to this fork, not the
   original — that's what lets you push without needing anyone to grant
   you access first.

4. Clone **your fork** (not the original — check the URL has your username
   in it):
   ```
   git clone https://github.com/<your-github-username>/SupportTicketDashboard_StarterPack.git
   cd SupportTicketDashboard_StarterPack
   ```

5. If you've never used git on this machine before, set your identity
   (skip this if you already know it's configured):
   ```
   git config --global user.name "Your Name"
   git config --global user.email "you@example.com"
   ```

6. Create your own branch, named after you — first name, underscore, last
   name (replace any spaces or hyphens in your name with underscores too),
   e.g. `Jane_Doe`:
   ```
   git checkout -b Jane_Doe
   ```
   Replace `Jane_Doe` with your own name. Then confirm you're actually on
   it:
   ```
   git branch
   ```
   The line with a `*` next to it is your current branch — make sure it's
   yours, not `main`.

7. Confirm the project runs, so you know your machine is good before the
   clock starts:
   ```
   cd Backend
   dotnet run
   cd ..
   ```
   This should build the project and print ticket data. If instead you get
   an error like `'dotnet' is not recognized` or `No .NET SDKs were found`,
   the .NET SDK isn't installed system-wide on this machine. Don't try to
   install it yourself — instead, from the repo root, run:
   ```
   .\tools\setup-dotnet.ps1
   ```
   (macOS/Linux: `source ./tools/setup-dotnet.sh` — the `source` matters).
   This activates a portable copy of the SDK for just this terminal window,
   with no admin rights or install needed. Then retry step 7. If that
   script also can't find a portable SDK to use, ask a proctor — don't
   spend your own time chasing it further.

8. **Start your 2.5-hour timer now.** Everything from here on (Phases 1–3)
   is timed.

9. While you work, save your progress as often as you like:
   ```
   git add .
   git commit -m "describe what you changed"
   ```

10. When you're done (or time is up), push your branch to your fork:
    ```
    git push -u origin Jane_Doe
    ```
    (Replace `Jane_Doe` with your branch name. `origin` here is your own
    fork, since that's what you cloned — you only need `-u origin
    Jane_Doe` the first time; after that, plain `git push` is enough.)

11. Open a Pull Request: on your fork's GitHub page you'll see a banner
    offering to open a PR from your new branch. Click it, confirm the base
    repository is `SkyeSenatla/SupportTicketDashboard_StarterPack` (base
    branch `main`), title it with your name, and submit it. **That Pull
    Request is your submission.** Tell your proctor/interviewer once it's
    open. Remember: don't go looking at anyone else's PR.

## How to check your work

- **Backend:** open a terminal in the `Backend` folder and run:
  ```
  dotnet run
  ```
  This prints the output of every `TicketManager` method. Compare it against
  what the method's job should be (see the comments above each method in
  `TicketManager.cs`).
- **Frontend:** open `Frontend/index.html` directly in a browser (double-click
  it, or use VS Code's "Open with Live Server" / "Reveal in File Explorer").
  No server needs to be running — it's a static page. Open the browser's
  DevTools console (F12) too — several `script.js` functions aren't wired to
  any button and are only checked by comparing their logged output against
  the comment above each function, the same way you check the backend.

## Checklist

### Phase 1 — C# Backend (`Backend/TicketManager.cs`)

**Warm-up**

- [ ] `GetHighPriorityTickets()` returns only tickets with `PriorityLevel`
      `"Critical"` or `"High"`
- [ ] `SortTicketsByDate()` returns the tickets sorted newest → oldest by
      `CreatedDate`

**Core**

- [ ] `GetAllTickets()` returns a defensive copy — a caller mutating the
      returned list must not affect `TicketManager`'s internal state
- [ ] `GetTicketCountsByStatus()` returns a `Dictionary<string, int>` with
      the correct count of tickets per status (`Open`, `In Progress`,
      `Closed`), implemented with LINQ rather than a manual loop
- [ ] `SortTicketsByPriorityThenDate()` orders tickets by urgency
      (`Critical` → `High` → `Medium` → `Low`), then by newest `CreatedDate`
      within the same priority
- [ ] `GetAverageResolutionDays()` returns the average days between
      `CreatedDate` and `ClosedDate` across closed tickets only, and returns
      `0` (not a crash) when there are none
- [ ] `GetTicketsByAssignee(assignee)` matches `AssignedTo` case-insensitively,
      and treats `null`/empty input as "give me the unassigned tickets"
- [ ] `SearchTickets(keyword)` matches the keyword case-insensitively against
      a ticket's `Title`, `Tags`, and `Comments`

**Reasoning**

- [ ] `GetSlaBreaches(asOf)` returns unresolved tickets whose age
      (`asOf - CreatedDate`) exceeds their priority's SLA threshold
      (`SlaThresholds`); Closed tickets never breach, no matter how old
- [ ] `GetEscalatedTickets(asOf)` returns **new** ticket copies for every
      SLA-breaching ticket, with priority bumped one step more urgent
      using `PriorityOrder` (Critical stays Critical) — the originals in
      `_tickets` must be untouched

**Debug it**

- [ ] `GetUnresolvedTickets()` is fully implemented but returns the wrong
      tickets — find the bug (compare its output against its doc comment)
      and fix it without rewriting the method

### Phase 2 — HTML & CSS (`Frontend/index.html`, `Frontend/style.css`)

- [ ] The page loads completely unstyled and completely non-interactive:
      add the missing `<link>` tag (in `<head>`) so `style.css` is applied,
      and the missing `<script>` tag so `script.js` runs. Think about where
      the script tag needs to go — it looks up elements like
      `#ticket-container` as soon as it runs
- [ ] The "Example Ticket Card" in `index.html` has the classes it needs
      (check `style.css` for the class names it expects) so it renders
      styled like a real ticket card
- [ ] The header lays out the title and filter buttons in a row, not stacked
      (`.dashboard-header`, `.filter-panel` in `style.css`)
- [ ] The ticket list below the example card lays out as a responsive
      multi-column grid, not a single stacked column (`.ticket-grid` in
      `style.css`)
- [ ] `.badge-critical`, `.badge-high`, `.badge-medium`, `.badge-low` each
      have a background color and text color that make sense for their
      urgency (critical = most urgent, low = least)

### Phase 3 — JavaScript (`Frontend/script.js`)

**Warm-up**

- [ ] `renderTickets()` builds a ticket card for each ticket and appends it
      to the `#ticket-container` element (match the structure of the
      Example Ticket Card)
- [ ] Each rendered card's badge and status get the correct CSS class
      based on the ticket's `priorityLevel` and `status` (use the
      `getBadgeClass()` / `getStatusClass()` helpers already provided)
- [ ] The three filter buttons (`Show All`, `Show Critical`, `Show Open`)
      are wired up with click listeners so they actually filter the list

**Core** (verify these via the browser console — see "How to check your work")

- [ ] `getTicketCountsByStatus(tickets)` returns a plain object with the
      correct count of tickets per status, implemented with `reduce()`
      rather than a manual loop
- [ ] `sortTicketsByPriorityThenDate(tickets)` returns a **new** array
      ordered by urgency (`Critical` → `High` → `Medium` → `Low`), then by
      newest `createdDate` within the same priority
- [ ] `getAverageResolutionDays(tickets)` returns the average days between
      `createdDate` and `closedDate` across closed tickets only, and
      returns `0` (not `NaN`) when there are none
- [ ] `getTicketsByAssignee(tickets, assignee)` matches `assignedTo`
      case-insensitively, and treats `null`/empty input as "give me the
      unassigned tickets"
- [ ] `searchTickets(tickets, keyword)` matches the keyword
      case-insensitively against a ticket's `title`, `tags`, and `comments`

**Debug it**

- [ ] `getTicketsSortedByDate(tickets)` is fully implemented but has a bug:
      calling it permanently reorders the shared `tickets` array instead of
      only returning a sorted copy. Find the line responsible and fix it
      without rewriting the function

## Done?

When your dashboard matches the screenshots above and every checklist item
is checked, you're done. Good luck.
