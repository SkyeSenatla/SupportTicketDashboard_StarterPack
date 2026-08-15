// Mock data simulating what would come from the C# backend's TicketManager.
const tickets = [
  { id: 1, title: "Email server down", status: "Open", priorityLevel: "Critical", createdDate: "2026-08-10", closedDate: null, assignedTo: "Maya Patel", tags: ["email", "outage"], comments: [{ author: "Maya Patel", text: "Escalated to infra team.", timestamp: "2026-08-10" }] },
  { id: 2, title: "Printer offline on 3rd floor", status: "Open", priorityLevel: "Low", createdDate: "2026-08-05", closedDate: null, assignedTo: null, tags: ["hardware", "printer"], comments: [] },
  { id: 3, title: "VPN keeps disconnecting", status: "In Progress", priorityLevel: "High", createdDate: "2026-08-09", closedDate: null, assignedTo: "Jordan Lee", tags: ["network", "vpn"], comments: [{ author: "Jordan Lee", text: "Reproduced on Windows clients only.", timestamp: "2026-08-09" }] },
  { id: 4, title: "New hire laptop setup", status: "Closed", priorityLevel: "Medium", createdDate: "2026-07-28", closedDate: "2026-07-30", assignedTo: "Maya Patel", tags: ["onboarding", "hardware"], comments: [{ author: "Maya Patel", text: "Laptop imaged and delivered.", timestamp: "2026-07-30" }] },
  { id: 5, title: "Database replication lag", status: "Open", priorityLevel: "Critical", createdDate: "2026-08-12", closedDate: null, assignedTo: "Jordan Lee", tags: ["database", "performance"], comments: [] },
  { id: 6, title: "Password reset request", status: "Closed", priorityLevel: "Low", createdDate: "2026-07-30", closedDate: "2026-07-30", assignedTo: null, tags: ["account"], comments: [{ author: "Helpdesk Bot", text: "Auto-resolved via self-service portal.", timestamp: "2026-07-30" }] },
  { id: 7, title: "Office Wi-Fi intermittent", status: "In Progress", priorityLevel: "Medium", createdDate: "2026-08-06", closedDate: null, assignedTo: null, tags: ["network", "wifi"], comments: [{ author: "Sam Osei", text: "Checking access point firmware.", timestamp: "2026-08-07" }] },
  { id: 8, title: "Payroll app throwing 500s", status: "Open", priorityLevel: "High", createdDate: "2026-08-11", closedDate: null, assignedTo: "Sam Osei", tags: ["payroll", "bug"], comments: [{ author: "Sam Osei", text: "Stack trace points to a null reference in the payroll service.", timestamp: "2026-08-11" }] },
];

// Lower rank = more urgent. Used for priority-aware sorting.
const PRIORITY_RANK = { Critical: 0, High: 1, Medium: 2, Low: 3 };

function getBadgeClass(priorityLevel) {
  return "badge-" + priorityLevel.toLowerCase();
}

function getStatusClass(status) {
  return "status-" + status.toLowerCase().replace(" ", "-");
}

function renderTickets(ticketsToRender) {
  const container = document.getElementById("ticket-container");
  container.innerHTML = "";

  ticketsToRender.forEach(ticket => {
    const card = document.createElement("article");
    card.className = "ticket-card";

    const header = document.createElement("div");
    header.className = "ticket-card-header";

    const title = document.createElement("h3");
    title.className = "ticket-title";
    title.textContent = ticket.title;

    const badge = document.createElement("span");
    badge.className = "badge " + getBadgeClass(ticket.priorityLevel);
    badge.textContent = ticket.priorityLevel;

    header.appendChild(title);
    header.appendChild(badge);

    const meta = document.createElement("div");
    meta.className = "ticket-meta";

    const status = document.createElement("span");
    status.className = "ticket-status " + getStatusClass(ticket.status);
    status.textContent = ticket.status;

    const date = document.createElement("span");
    date.className = "ticket-date";
    date.textContent = ticket.createdDate;

    meta.appendChild(status);
    meta.appendChild(date);

    card.appendChild(header);
    card.appendChild(meta);

    container.appendChild(card);
  });
}

function setActiveButton(activeId) {
  document.querySelectorAll(".filter-btn").forEach(btn => {
    btn.classList.remove("active");
  });
  document.getElementById(activeId).classList.add("active");
}

function showAllTickets() {
  renderTickets(tickets);
  setActiveButton("filter-all");
}

function showCriticalTickets() {
  renderTickets(tickets.filter(t => t.priorityLevel === "Critical"));
  setActiveButton("filter-critical");
}

function showOpenTickets() {
  renderTickets(tickets.filter(t => t.status === "Open"));
  setActiveButton("filter-open");
}

// Wire up the three filter buttons with click listeners.
document
  .getElementById("filter-all")
  .addEventListener("click", showAllTickets);

document
  .getElementById("filter-critical")
  .addEventListener("click", showCriticalTickets);

document  
  .getElementById("filter-open")
  .addEventListener("click", showOpenTickets);

renderTickets(tickets);

// ---------------------------------------------------------------------
// The functions below aren't wired to any button - they're pure data
// helpers, verified through the browser console (see README).
// ---------------------------------------------------------------------

// Counts tickets per status. Returns a plain object like
// { "Open": 4, "In Progress": 2, "Closed": 2 }.
function getTicketCountsByStatus(ticketsToCount) {
  return ticketsToCount.reduce((counts, ticket) => {
    counts[ticket.status] = (counts[ticket.status] || 0) + 1;
    return counts;
  }, {});
}

// Returns a NEW array ordered by urgency first (Critical, then High, then
// Medium, then Low), and within the same priority, newest createdDate
// first. Must not mutate the input array.
function sortTicketsByPriorityThenDate(ticketsToSort) {
  return [...ticketsToSort].sort((a, b) => {
    const priorityDiff = PRIORITY_RANK[a.priorityLevel] - PRIORITY_RANK[b.priorityLevel];
    if (priorityDiff !== 0) return priorityDiff;
    return new Date(b.createdDate) - new Date(a.createdDate);
  });
}

// Returns the average number of days between createdDate and closedDate
// for tickets that have a closedDate. Tickets without a closedDate must be
// excluded. Return 0 if there are no closed tickets (don't divide by zero!).
function getAverageResolutionDays(ticketsToAverage) {
  const closed = ticketsToAverage.filter(t => t.closedDate);
  if (closed.length === 0) return 0;

  const totalDays = closed.reduce((sum, t) => {
    const msDiff = new Date(t.closedDate) - new Date(t.createdDate);
    return sum + msDiff / (1000 * 60 * 60 * 24);
  }, 0);

  return totalDays / closed.length;
}

// Returns tickets assigned to the given person (case-insensitive match on
// assignedTo). Passing null or an empty string should return the tickets
// that are currently unassigned.
function getTicketsByAssignee(ticketsToFilter, assignee) {
  if (!assignee) {
    return ticketsToFilter.filter(t => !t.assignedTo);
  }
  return ticketsToFilter.filter(
    t => t.assignedTo && t.assignedTo.toLowerCase() === assignee.toLowerCase()
  );
}

// Returns tickets where the keyword (case-insensitive) appears in the
// title, in any of the ticket's tags, or in the text of any comment.
function searchTickets(ticketsToSearch, keyword) {
  if (!keyword) return [];
  const kw = keyword.toLowerCase();

  return ticketsToSearch.filter(t => {
    const inTitle = t.title.toLowerCase().includes(kw);
    const inTags = t.tags.some(tag => tag.toLowerCase().includes(kw));
    const inComments = t.comments.some(c => c.text.toLowerCase().includes(kw));
    return inTitle || inTags || inComments;
  });
}

// Returns a NEW array of tickets sorted newest to oldest by createdDate.
function getTicketsSortedByDate(ticketsToSort) {
  // BUG (original): called .sort() directly on `ticketsToSort`. Array.sort()
  // sorts IN PLACE and returns a reference to that same array, so this was
  // silently reordering the caller's original array (here, the shared
  // `tickets` array) instead of returning a new sorted copy.
  // FIX: spread into a new array first, then sort that copy.
  return [...ticketsToSort].sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate));
}

console.log("=== Ticket Counts By Status ===");
console.log(getTicketCountsByStatus(tickets));

console.log("=== Tickets Sorted By Priority, Then Newest ===");
sortTicketsByPriorityThenDate(tickets).forEach(t => {
  console.log(`[${t.priorityLevel}] ${t.createdDate} - #${t.id} ${t.title}`);
});

console.log("=== Average Resolution Time (Days) ===");
console.log(getAverageResolutionDays(tickets));

console.log("=== Tickets Assigned To Maya Patel ===");
getTicketsByAssignee(tickets, "Maya Patel").forEach(t => console.log(`#${t.id} ${t.title}`));

console.log("=== Unassigned Tickets ===");
getTicketsByAssignee(tickets, null).forEach(t => console.log(`#${t.id} ${t.title}`));

console.log('=== Search Results For "network" ===');
searchTickets(tickets, "network").forEach(t => console.log(`#${t.id} ${t.title}`));

console.log("=== Ticket Order Before Sorting By Date (should be #1..#8) ===");
console.log(tickets.map(t => t.id).join(", "));
console.log("=== Tickets Sorted Newest To Oldest ===");
getTicketsSortedByDate(tickets).forEach(t => console.log(`${t.createdDate} - #${t.id} ${t.title}`));
console.log("=== Ticket Order After Sorting By Date (should still be #1..#8) ===");
console.log(tickets.map(t => t.id).join(", "));
