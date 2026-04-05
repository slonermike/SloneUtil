# AssetManagement

Runtime object pooling for memory-efficient instantiation.

## Files

### [ObjectPool.cs](ObjectPool.cs) — `ObjectPool<T>`

Generic object pool using linked lists for O(1) allocation and page-based expansion.

- `Allocate()` — Get an object from the pool. Automatically expands by `pageSize` if empty.
- `Free(node)` — Return an object to the pool.
- Supports an optional creation handler callback for initializing new objects.
- Tracks `numAllocated` for diagnostics.
