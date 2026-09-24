# Boxing Position Workflow

## Flow Overview

### Khi scan BoxingPosition A1:

```
Scan A1
   ↓
A1 tồn tại?
   ↓
YES
   ↓
Lấy tất cả Job đang InProgress của A1
   ↓
Trả danh sách cho frontend
```

**Ví dụ Response:**

```json
{
  "positionId": 1,
  "positionCode": "A1",
  "activeJobs": [
    {
      "jobId": 101,
      "jobNo": "JOB-001",
      "batchItemId": 10,
      "lotNo": "0103567533"
    },
    {
      "jobId": 102,
      "jobNo": "JOB-002",
      "batchItemId": 11,
      "lotNo": "0103568000"
    }
  ]
}
```

**Frontend hiển thị:**

```
BOXING POSITION A1

Đang có:

JOB-001
Lot: 0103567533

JOB-002
Lot: 0103568000
```

---

## Boxing Job & BatchItem Relationship

**Entity:**

```csharp
public class BoxingJob : EntityAuditBase
{
    public string JobNo { get; set; }
    public BoxingJobStatus Status { get; set; }
    public int ActualQty { get; set; }

    public int BoxingPositionId { get; set; }
    public BoxingPosition BoxingPosition { get; set; }

    public int BatchItemId { get; set; }
    public BatchItem BatchItem { get; set; }

    public ICollection<Box> Boxes { get; set; } = [];
}
```

**Hierarchy:**

```
A1
│
├── Job001 → BatchItem/Lot A
│      ├── Box001
│      └── Box002
│
├── Job002 → BatchItem/Lot B
│      ├── Box003
│      └── Box004
│
└── Job003 → BatchItem/Lot C
       └── Box005
```

---

## Scanning Flow

### Khi Worker scan Box

```
Scan Position A1
       ↓
Scan Box (BOX001)
       ↓
Box thuộc Job nào?
       ↓
Xác định Job (JOB-001 → Lot A)
       ↓
Scan Tag
       ↓
Check Tag.Lot == Job.BatchItem.Lot
```

### Box mới

```
Scan Box mới (BOX010)
       ↓
Box chưa có Job
       ↓
Scan Tag đầu tiên (Lot B)
       ↓
Tag xác định Lot
       ↓
Tìm Job InProgress của A1 + BatchItem/Lot B
       ↓
Có → dùng Job hiện tại (Job002)
Không → tạo Job mới
```

---

## Validation Rules

**Khi scan Tag:**
- Tag.Lot phải == Box.Job.BatchItem.Lot
- Nếu không → ERROR: Tag không cùng Lot với Box

**Ví dụ:**

```
A1 có:
  Job001 - Lot A
  Job002 - Lot B

Worker scan A1
Worker scan BOX010 (new box)
Worker scan Tag Lot B

→ BOX010 được gắn vào Job002 (hệ thống tự xác định)
```

Worker không cần biết Job002 là gì. Hệ thống xác định thông qua Box + Lot của Tag.
