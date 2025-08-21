# Inventory Management Tests

Test projects for the Inventory Reservation System.

## Test Structure

The tests are organized to match the Clean Architecture layers:
- Domain tests: Verify business rules and entity behavior
- Application tests: Validate use cases and command/query handlers
- Infrastructure tests: Ensure proper data persistence
- API tests: Verify HTTP endpoints and responses

## Testing Approach

- Unit tests for domain logic and business rules
- Integration tests for repository implementations
- Controller tests for API endpoints
- Mock-based testing for external dependencies