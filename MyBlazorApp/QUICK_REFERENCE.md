# EventEase - Quick Reference Guide

## Data Binding Quick Reference

### Binding Syntax Patterns

| Pattern | Example | Use Case |
|---------|---------|----------|
| **Simple Property** | `@Event.Name` | Display string/value properties |
| **Method Invocation** | `@GetCapacityPercentage()` | Computed/calculated values |
| **Format String** | `@Event.Date.ToString("MMM dd, yyyy")` | Date/number formatting |
| **Ternary Operator** | `@(IsEventFull() ? "Full" : "Register")` | Conditional display |
| **Array/List Index** | `@events[0].Name` | Access collection items |
| **ForEach Loop** | `@foreach (var e in events)` | Iterate collections |
| **Attribute Binding** | `disabled="@IsEventFull()"` | Set HTML attributes |
| **Style Binding** | `style="width: @percentage%"` | Dynamic styles |
| **Event Binding** | `@onclick="OnClick"` | Handle events |

### EventCard Component Binding Examples

```razor
<!-- Property Binding -->
<h3>@Event.Name</h3>
<p>@Event.Description</p>

<!-- Formatted Property Binding -->
<p>@Event.Date.ToString("MMMM dd, yyyy - h:mm tt", CultureInfo.CurrentCulture)</p>

<!-- Computed Method Binding -->
<div class="progress-fill" style="width: @GetCapacityPercentage()%"></div>

<!-- Ternary/Conditional Binding -->
<button disabled="@IsEventFull()">
    @(IsEventFull() ? "Event Full" : "Register Now")
</button>

<!-- Collection Binding in Parent -->
@foreach (var eventItem in events)
{
    <EventCard Event="@eventItem" OnRegister="HandleRegisterEvent" />
}
```

### Parameter Binding (Component Communication)

```csharp
// In EventCard.razor.cs
[Parameter]
public Event Event { get; set; } = new();  // Receive event data

[Parameter]
public EventCallback OnRegister { get; set; }  // Callback pattern

// Usage from parent (Events.razor)
<EventCard Event="@eventItem" OnRegister="HandleRegisterEvent" />
```

---

## Routing Quick Reference

### Route Declaration

```razor
<!-- Simple route -->
@page "/events"

<!-- Route with parameter -->
@page "/event/{EventId:int}"

<!-- Multiple routes (same component) -->
@page "/events"
@page "/event-list"
```

### Route Parameters

```csharp
// Route parameter binding
[Parameter]
public int EventId { get; set; }

// Accessed in code
protected override async Task OnInitializedAsync()
{
    var evt = GetEventById(EventId);  // Use parameter
}
```

### Route Constraints

| Constraint | Format | Example |
|-----------|--------|---------|
| Integer | `{id:int}` | `/event/{EventId:int}` |
| GUID | `{id:guid}` | `/report/{ReportId:guid}` |
| String | `{name}` | `/user/{UserName}` |
| Boolean | `{active:bool}` | `/items/{IsActive:bool}` |

### Navigation Methods

```razor
<!-- NavLink (declarative) -->
<NavLink href="/events">Browse Events</NavLink>
<NavLink href="@GetEventDetailUrl()" class="btn">View Details</NavLink>

<!-- Programmatic Navigation -->
@inject NavigationManager Navigation

<button @onclick="GoBack">Back</button>

@code {
    private void GoBack()
    {
        Navigation.NavigateTo("/events");
    }
    
    private string GetEventDetailUrl() => $"/event/{Event.Id}";
}
```

---

## EventEase Routes

### Application Routes

```
┌─────────────────────────────────────────┐
│              /                          │
│  Home - Landing page                    │
│  - Hero section                         │
│  - Feature highlights                   │
│  - "Browse Events" CTA                  │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│            /events                      │
│  Events List - Event browsing           │
│  - Display all upcoming events          │
│  - EventCard grid layout                │
│  - Quick register buttons               │
│  - Links to event details               │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│         /event/{EventId}                │
│  Event Details - Full event page        │
│  - Complete event information           │
│  - Large registration section           │
│  - Back button to /events               │
│  - Support contact information          │
└─────────────────────────────────────────┘
```

---

## Data Flow Example: Click "View Details"

```
1. EventCard Component
   └─ Click "View Details" button
   └─ NavLink href="@GetEventDetailUrl()"
   └─ Method returns: "/event/1"

2. Router receives "/event/1"
   └─ Matches: @page "/event/{EventId:int}"
   └─ Extracts: EventId = 1
   └─ Loads: EventDetail.razor component

3. EventDetail Component
   └─ [Parameter] EventId = 1
   └─ OnInitializedAsync() called
   └─ GetEventById(1) looks up event data
   └─ Component renders with event data
   └─ All @EventDetail.Name, @currentEvent.Date, etc. bind

4. Back Button
   └─ @onclick="GoBack"
   └─ NavigationManager.NavigateTo("/events")
   └─ Returns to Events.razor
```

---

## Common Binding Scenarios

### Scenario 1: Display Formatted Date

```csharp
// Model has: public DateTime Date { get; set; }

// In component:
<p>@Event.Date.ToString("MMMM dd, yyyy - h:mm tt", CultureInfo.CurrentCulture)</p>

// Produces: "April 15, 2026 - 09:00 AM"
```

### Scenario 2: Conditional Button State

```csharp
// Method: private bool IsEventFull() => Event.RegisteredAttendees >= Event.Capacity;

// In component:
<button disabled="@IsEventFull()" 
        class="btn-register">
    @(IsEventFull() ? "Event Full" : "Register Now")
</button>

// States:
// - If full: button is disabled, shows "Event Full"
// - If available: button is enabled, shows "Register Now"
```

### Scenario 3: Dynamic Progress Bar

```csharp
// Method:
private string GetCapacityPercentage()
{
    if (Event.Capacity == 0) return "0";
    return ((Event.RegisteredAttendees * 100) / Event.Capacity).ToString();
}

// In component:
<div class="progress-fill" style="width: @GetCapacityPercentage()%"></div>

// Updates:
// For 342/500: width = 68.4%
// For 156/200: width = 78%
// For 150/150: width = 100%
```

### Scenario 4: List with Event Binding

```csharp
// In parent component:
private List<Event>? events;

protected override async Task OnInitializedAsync()
{
    events = GetSampleEvents();  // Load from service/data
}

// In markup:
@foreach (var eventItem in events)
{
    <EventCard Event="@eventItem" 
               OnRegister="HandleRegisterEvent" />
}

// Flow:
// - Iterate events list
// - Pass each event to EventCard
// - EventCard displays bound properties
// - User sees grid of event cards
```

---

## Best Practices Implemented

✅ **Data Binding**
- Always initialize optional parameters: `public Event Event { get; set; } = new();`
- Use type-safe string formatting instead of concatenation
- Implement null checks in conditionals: `@if (events != null)`
- Use methods for computed values instead of inline logic
- Format dates with `CultureInfo.CurrentCulture` for localization

✅ **Routing**
- Use type constraints: `{id:int}` instead of `{id}`
- Encapsulate route generation: `GetEventDetailUrl()` method
- Use semantic route names: `/events`, `/event/{id}`
- Implement breadcrumb navigation (back buttons)
- Use relative paths in NavLink when possible
- Inject `NavigationManager` for programmatic navigation

✅ **Component Communication**
- Use `[Parameter]` for parent-to-child data
- Use `EventCallback<T>` for child-to-parent communication
- Initialize default parameter values
- Validate parameters in lifecycle methods

---

## Debugging Tips

### Check Data Binding
```csharp
// Add temporary debug output
<p>DEBUG: Event.Name = "@Event.Name"</p>
<p>DEBUG: Capacity = @Event.RegisteredAttendees / @Event.Capacity</p>
```

### Check Route Parameters
```csharp
// Add in component code
protected override async Task OnInitializedAsync()
{
    System.Diagnostics.Debug.WriteLine($"EventId = {EventId}");
    await base.OnInitializedAsync();
}
```

### Verify Navigation
```csharp
// Check URL in browser address bar
// Verify component parameters receive correct values
// Use browser DevTools Network tab to see route changes
```

### Component State
```csharp
// Ensure Property attributes exist:
// [Parameter] int EventId { get; set; }
// [Parameter] EventCallback OnRegister { get; set; }
```

---

## Related Files

- Data Model: [Models/Event.cs](Models/Event.cs)
- Component Code: [Components/EventCard.razor](Components/EventCard.razor)
- Events Page: [Pages/Events.razor](Pages/Events.razor)
- Detail Page: [Pages/EventDetail.razor](Pages/EventDetail.razor)
- Full Guide: [BINDING_AND_ROUTING_GUIDE.md](BINDING_AND_ROUTING_GUIDE.md)
- Styles: [Components/EventCard.razor.css](Components/EventCard.razor.css)

---

## Status

✅ **Build**: Successful - 0 errors, 0 warnings  
✅ **Data Binding**: Fully implemented with multiple patterns  
✅ **Routing**: Complete with type-safe parameters  
✅ **Navigation**: Working between all pages  
✅ **Ready for Activity 2**: Debugging and optimization
