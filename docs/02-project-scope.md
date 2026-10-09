# Warehouse Management System — Project Scope

**Status:** Approved Baseline v0.1

## 1. System Boundary

The system manages warehouse inventory operations for a fictional Saudi distribution company operating warehouses in Riyadh, Jeddah, and Dammam.

The system focuses on physical stock tracking, storage locations, operational workflows, and inventory integrity. It is not intended to replace a full ERP, accounting, or transportation management system.

## 2. MVP Scope

### Master Data
- Products and basic product categories.
- Suppliers.
- Warehouses and storage locations (bins).
- Basic users and roles.

### Inbound Operations
- Create and approve basic purchase orders.
- Receive goods against approved purchase orders.
- Support partial deliveries.
- Record accepted, damaged, and rejected quantities.
- Put away accepted goods into warehouse bins.

### Inventory
- Track stock by product, warehouse, and storage location.
- Distinguish sellable/allocatable stock from damaged or quarantined stock.
- Maintain traceable stock movement records.
- Prevent duplicate posting of receiving operations.
- Enforce transactional consistency for stock-changing operations.

## 3. V1.0 Scope

### Reservations
- Reserve available stock for outbound requests.
- Prevent over-reservation under concurrent requests.
- Release reservations when requests are cancelled or quantities are reduced.

### Simplified Outbound Fulfillment
- Create outbound requests.
- Validate stock availability.
- Allocate and reserve stock.
- Confirm picked quantities.
- Handle picking shortages and partial fulfillment according to documented business rules.
- Confirm dispatch and record stock deductions.
- Support cancellation before dispatch under defined conditions.

Advanced package management, carrier integrations, and shipping label generation are excluded.

### Inter-Warehouse Transfers
- Request and approve transfers.
- Dispatch stock from the source warehouse.
- Track stock in transit.
- Receive stock at the destination warehouse.
- Record and reconcile transfer discrepancies.

### Inventory Control
- Record physical stock counts.
- Identify stock variances.
- Require approval for stock adjustments.
- Preserve an auditable history of adjustments.

### Security and Quality
- Role-based access control.
- Audit logging for critical operations.
- Unit and integration tests.
- Concurrency tests for critical inventory workflows.
- API documentation.
- CI/CD with GitHub Actions.
- Docker-based deployment.
- Performance testing with k6.

## 4. Future Enhancements

- Advanced picking and packing workflows.
- Lot/batch and serial number tracking.
- Expiration dates and FEFO allocation.
- Advanced replenishment strategies.
- Carrier and ERP integrations.
- Advanced analytics and forecasting.
- React + TypeScript frontend.

## 5. Business Assumptions

- The company and its operational policies are fictional.
- The initial warehouse network contains three warehouses.
- Products are initially tracked by quantity, without lot or serial tracking.
- The first end-to-end workflow will be validated using one warehouse.
- Inventory quantities cannot become negative through normal operational workflows.
- Specific policies for receiving tolerances, stock status, reservation priority, partial fulfillment, and discrepancy resolution will be defined during Use Case analysis.

## 6. Acceptance of Scope

A feature is complete only when:
- Its business rules and acceptance criteria are documented.
- Its implementation is functional.
- Relevant automated tests pass.
- Inventory-changing behavior preserves data consistency.
- Required documentation is updated.

V1.0 is complete only when its agreed workflows have been implemented and verified. Unimplemented features must not be presented as completed.
