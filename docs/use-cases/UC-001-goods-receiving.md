# UC-001 — Receive Goods Against Purchase Order

**Status:** Draft v0.1
**Release:** MVP

## 1. Business Goal

Receive goods against an approved purchase order, record the physical quantities and inspection results, and make accepted goods available only after putaway into an eligible storage bin.

## 2. Actors

- Warehouse Operator
- Warehouse Manager (exception handling and oversight)

## 3. Preconditions

- An approved Purchase Order exists.
- The Purchase Order belongs to the receiving warehouse.
- The Purchase Order contains at least one item with an outstanding receivable quantity.
- The receiving operator has the required permission.

## 4. Main Workflow

1. The operator selects an approved Purchase Order.
2. The system displays ordered, previously received, and outstanding quantities for each item.
3. The operator records the quantities physically received.
4. The operator records inspection results: accepted, damaged, and rejected quantities.
5. The system validates that the inspection quantities equal the physically received quantities.
6. The system validates receiving limits against the Purchase Order.
7. The operator confirms the Goods Receipt.
8. The system posts the receipt transaction and records the goods in their appropriate non-allocatable receiving or holding locations.
9. Accepted goods are subsequently put away into eligible storage bins.
10. The system updates inventory balances and records the relevant stock movements.
11. Goods become available for reservation only after successful putaway into an eligible bin.

## 5. Business Rules

**BR-001:** A Purchase Order may have multiple Goods Receipts (partial receiving).

**BR-002:** Goods Receipt quantities must be positive and must not exceed the outstanding receivable quantity for the corresponding Purchase Order line. Over-receipt tolerance is excluded from the MVP.

**BR-003:** For each receipt line:
Physically Received = Accepted + Damaged + Rejected.

**BR-004:** Rejected goods must be segregated in a designated holding area and must not be available for reservation or dispatch.

**BR-005:** Damaged goods must not be available for reservation or normal dispatch.

**BR-006:** Accepted goods remain non-allocatable until successfully put away into an eligible bin.

**BR-007:** Purchase Order approval alone does not increase physical inventory.

**BR-008:** Confirming the same Goods Receipt more than once must not duplicate inventory quantities or stock movements.

**BR-009:** Inventory-changing operations must be atomic: either all required changes are committed or none are.

**BR-010:** Every posted inventory quantity change must be traceable to its originating business operation.

## 6. Example

Purchase Order: 100 laptops

First delivery:
- Physically received: 95
- Accepted: 90
- Damaged: 3
- Rejected: 2
- Outstanding receivable quantity: 5

After receiving and before putaway:
- 90 accepted laptops are in a non-allocatable receiving location.
- 3 damaged laptops are held separately.
- 2 rejected laptops are held separately.
- None of these 95 laptops is available for normal reservation.

After putaway of all 90 accepted laptops:
- 90 laptops become available for reservation, assuming no other restrictions.
- 3 damaged and 2 rejected laptops remain unavailable.
- The Purchase Order may receive the remaining 5 laptops in another Goods Receipt.

## 7. Exception Scenarios

- Purchase Order is not approved: reject the receipt operation.
- Receipt quantity exceeds outstanding quantity: reject the operation.
- Inspection quantities do not match physical receipt: reject confirmation.
- Duplicate receipt confirmation: do not post inventory twice.
- Putaway destination is not eligible: reject putaway.
- Partial putaway: only the quantity successfully put away becomes available.

## 8. Acceptance Criteria

- An approved Purchase Order supports multiple partial receipts.
- The system prevents receiving quantities above the outstanding limit.
- Inspection quantity totals are validated.
- Damaged and rejected stock cannot be reserved.
- Accepted stock cannot be reserved before eligible putaway.
- Successful putaway updates balances and creates traceable stock movements.
- Repeated receipt confirmation does not duplicate stock.
- Failed stock-changing operations do not leave partial updates.

## 9. Open Design Questions

- Exact Goods Receipt lifecycle statuses and allowed transitions.
- Representation of receiving, damaged, and rejected holding locations.
- How outstanding receivable quantity is affected by rejected or damaged goods and supplier replacement arrangements.
- Exact putaway confirmation workflow and partial putaway handling.
- Concurrent receipts against the same Purchase Order line.

These questions will be resolved during detailed design before implementing the affected behavior.
