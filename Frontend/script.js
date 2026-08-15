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
    // TODO: Build the ticket card DOM structure and append it to `container`.
    // Match the structure of the "Example Ticket Card" in index.html:
    <article class="ticket-card">
       <div class="ticket-card-header">
         <h3 class="ticket-title">...</h3>
           <span class="badge ...">...</span>
       </div>
    <div class="ticket-meta">
      <span class="ticket-status ...">...</span>
     <span class="ticket-date">...</span>
        </div>
       </article>
    //
    // TODO: Dynamic styling - use getBadgeClass(ticket.priorityLevel) and
    // getStatusClass(ticket.status) to add the correct classes to the
    // badge and status elements so they're colored based on the ticket's data.
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

// TODO: Wire up the three filter buttons (#filter-all, #filter-critical,
// #filter-open) with click listeners that call showAllTickets(),
// showCriticalTickets() and showOpenTickets() respectively.

renderTickets(tickets);

// ---------------------------------------------------------------------
// The functions below aren't wired to any button - they're pure data
// helpers, verified through the browser console (see README).
// ---------------------------------------------------------------------

// Counts tickets per status. Returns a plain object like
// { "Open": 4, "In Progress": 2, "Closed": 2 }.
// TODO: Implement using reduce(), not a manual loop.
function getTicketCountsByStatus(ticketsToCount) {
  return {};
}

// Returns a NEW array ordered by urgency first (Critical, then High, then
// Medium, then Low), and within the same priority, newest createdDate
// first. Must not mutate the input array. Use the PRIORITY_RANK map above -
// sorting priorityLevel as a plain string will NOT give you the right order.
// TODO: Implement.
function sortTicketsByPriorityThenDate(ticketsToSort) {
  return [...ticketsToSort];
}

// Returns the average number of days between createdDate and closedDate
// for tickets that have a closedDate. Tickets without a closedDate must be
// excluded. Return 0 if there are no closed tickets (don't divide by zero!).
// TODO: Implement.
function getAverageResolutionDays(ticketsToAverage) {
  return 0;
}

// Returns tickets assigned to the given person (case-insensitive match on
// assignedTo). Passing null or an empty string should return the tickets
// that are currently unassigned.
// TODO: Implement. Remember assignedTo can itself be null - don't let a
// null assignedTo blow up your comparison.
function getTicketsByAssignee(ticketsToFilter, assignee) {
  return [];
}

// Returns tickets where the keyword (case-insensitive) appears in the
// title, in any of the ticket's tags, or in the text of any comment.
// TODO: Implement. You'll need some() to look inside the tags and comments
// arrays on each ticket.
function searchTickets(ticketsToSearch, keyword) {
  return [];
}

// Returns a NEW array of tickets sorted newest to oldest by createdDate.
//
// NOTE: This function is already fully written - but it has a bug. Compare
// its behavior against the description above (does it return a NEW array,
// or does calling it also change the order of the original `tickets`
// array?) and fix the one line responsible. Do not rewrite the function
// from scratch.
function getTicketsSortedByDate(ticketsToSort) {
  ticketsToSort.sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate));
  return ticketsToSort;
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
