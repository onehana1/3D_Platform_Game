# 3d 플랫폼 게임입니다.

---

| 기술 | 사용 버전 |
| --- | --- |
| 🎮 엔진 | Unity 2022.3.17f1 |
| 💻 언어 | C# |
| 🛠 개발 환경 | Visual Studio 2022 |

---

# 🏃‍♂️ **플레이어 컨트롤러 (Player Controller)**

이 프로젝트에서는 **Unity의 `InputAction` 시스템**을 활용하여 플레이어의 움직임을 구현하였습니다.

마우스를 사용하여 시점을 조정하고, 키보드 입력을 통해 다양한 동작을 수행할 수 있습니다.

---

## 🎮 **플레이어 이동 방식**

![image](https://github.com/user-attachments/assets/44b2a183-dfe9-411e-99ec-f5233adad0ce)


### 🔹 기본 이동

- `W` : 플레이어가 바라보는 방향으로 이동
- `S` : 뒤로 걷기 (후진)
- `A / D` : 좌우 회전
- 마우스 이동 : **카메라 회전 (3인칭 시점 조작)**

### 🔹 점프 & 대시

- `Space` : 점프 (`Rigidbody.AddForce`를 사용하여 구현)
- `Shift + W` : 뛰기 (이동 속도 증가)

---

## 🧱 **물리 시스템 및 감지 (Raycast 활용)**

### 🔹 **바닥 감지 (`IsGrounded`)**

- `Physics.CheckSphere`를 사용하여 플레이어가 **바닥에 닿아있는지 감지**합니다.
- 이를 통해 **점프 가능 여부**를 결정합니다.

### 🔹 **벽 감지 (`CheckWall`)**
![매달리기](https://github.com/user-attachments/assets/43ca173f-489b-4b5f-9310-d7edb5c81e0d)

- `Physics.SphereCast`를 활용하여 **플레이어가 벽에 접근하면 매달리는 기능**을 구현하였습니다.
- 벽을 감지하면 **중력을 비활성화**하여 플레이어가 벽에 붙을 수 있도록 합니다.

---

# 🎢 **플랫폼 및 트랩 시스템**

### 🔹 **점프대 (Launch Pad)**
![점프대](https://github.com/user-attachments/assets/88548eb4-4765-4f09-ba41-bc48c701eb33)

- 플레이어가 점프대에 올라가면 **`Rigidbody.AddForce`를 사용해 공중으로 발사**됩니다.
- 특정 방향으로 힘을 가해 플레이어를 날려보낼 수 있습니다.

### 🔹 **움직이는 플랫폼 (`MovePad`)**
![플랫폼](https://github.com/user-attachments/assets/be9425d0-d824-4852-b4de-dd2532c453b3)

- **수평(좌우) 또는 수직(상하)으로 이동하는 플랫폼**을 구현했습니다.
- `Mathf.PingPong`을 활용하여 일정한 거리만큼 반복적으로 이동합니다.
- `OnTriggerEnter`와 `OnTriggerExit`을 이용해 **플레이어가 플랫폼 위에 있을 때 함께 이동**하도록 설정했습니다.
    
    **구현 방식:**
    
    - `MovePadType.Horizontal` : 좌우 이동 (`X`축)
    - `MovePadType.Vertical` : 상하 이동 (`Y`축)
    - `OnTriggerEnter` : 플레이어가 플랫폼 위에 올라가면 **플랫폼의 자식 오브젝트로 설정**하여 함께 이동
    - `OnTriggerExit` : 플랫폼에서 벗어나면 **부모 관계 해제**

### 🔹 **트랩 시스템 (레이저 감지)**

- `Physics.Raycast`를 사용하여 특정 위치에 **레이저 센서**를 배치했습니다.
- 플레이어가 감지되면 **위에서 장애물이 떨어지는 트랩이 활성화**됩니다.
- 트랩은 일정 시간이 지나면 **자동으로 리셋**되어 다시 활성화됩니다.

---

# Unity 인벤토리 시스템
![도끼장착](https://github.com/user-attachments/assets/62542040-a85f-4272-856f-10da95142efa)

플레이어가 아이템을 획득하고, 장착하거나 사용하며, 드롭할 수 있는 기능이 구현되어 있습니다.

---

## 📂 **구조**

### 1️⃣ **주요 스크립트**

| 파일명 | 설명 |
| --- | --- |
| `InventoryManager.cs` | 아이템을 추가, 제거, 사용, 드롭 관리 |
| `UIInventory.cs` | 인벤토리 UI 관리 (토글, 업데이트, 아이템 선택) |
| `ItemSlot.cs` | 인벤토리 UI의 각 슬롯을 관리 |
| `ItemObject.cs` | 필드에 존재하는 아이템 오브젝트 |
| `PlayerController.cs` | 플레이어 입력 및 인벤토리 UI 토글 |
| `PlayerCondition.cs` | 플레이어 상태 (체력, 배고픔, 스태미나 등) |

---

## 🎮 **기능**

### 1️⃣ **아이템 획득**

- **필드에 있는 아이템을 상호작용 (`E` 키) 하면 인벤토리에 추가됨**

```csharp
public void OnInteract()
{
    InventoryManager.Instance.AddItem(data); // 인벤토리에 추가
    Destroy(gameObject);
}

```

### 2️⃣ **인벤토리 UI**
![image](https://github.com/user-attachments/assets/c3ec372a-88fb-46f3-8033-9efa266e6480)

- **`Tab` 키를 누르면 UI가 열리고 닫힘**
- **마우스로 슬롯을 클릭하면 아이템 선택됨**

```csharp
public void OnInventoryInput(InputAction.CallbackContext context)
{
    if (context.phase == InputActionPhase.Started)
    {
        inventory?.Invoke();
    }
}
```

### 3️⃣ **아이템 사용**

- **선택한 아이템이 소비 아이템이면 사용 가능**
- **`Use` 버튼 클릭 시 실행됨**

```csharp
ublic void OnUseButton()
{
    if (selectedItem == null || selectedItem.item == null) return;

    if (selectedItem.item.type == ItemType.Consumable)
    {
        InventoryManager.Instance.UseItem(selectedItem.item);
        UpdateUI();
        SetSelectItemClear();
    }
}
```

### 4️⃣ **아이템 장착 & 해제**

- **장비 아이템은 `Equip` 버튼을 눌러 장착 가능**
- **이미 장착된 경우 `UnEquip` 버튼을 눌러 해제 가능**

```csharp
public void OnEquipButton()
{
    if (selectedItem == null || selectedItem.item.type != ItemType.Equipable) return;
    CharacterManager.Instance.Player.equipment.EquipItem(selectedItem.item);
    UpdateUI();
    SetSelectItemClear();
}
```

### 5️⃣ **아이템 드롭**

- **선택한 아이템을 `Drop` 버튼을 눌러 필드에 드롭 가능**

```csharp
public void OnThrowButton()
{
    if (selectedItem == null || selectedItem.item == null) return;

    Vector3 dropPosition = CharacterManager.Instance.Player.transform.position +
                           CharacterManager.Instance.Player.transform.forward * 1.5f;
    InventoryManager.Instance.ThrowItem(selectedItem.item, dropPosition);
    UpdateUI();
}

```

# Unity 인벤토리 시스템 - 아이템 데이터 구조

**ScriptableObject**를 사용하여 아이템 데이터를 관리합니다.

이를 통해 다양한 아이템을 쉽게 생성하고 조정할 수 있습니다.

- **인벤토리 시스템과 연결**하여 쉽게 사용할 수 있습니다.

![image](https://github.com/user-attachments/assets/f97d7a39-252e-4ee2-a4ae-379cdd59430d)


## 🏷️ **아이템 종류 (Item Types)**

### 🔹 **소비 아이템 (Consumable)**

- **독버섯** 🍄 : 사용 시 **이동 속도가 증가**하지만 부작용이 있을 수도 있음.
- **스테미나 버섯** 🍄 : 사용 시 **스테미나 회복**.
- **배고픔을 회복시키는 풀** 🌿 : 사용 시 **배고픔(허기) 회복**.

### 🔹 **일반 자원 (Resource)**

- **돌** 🪨 : 조합(material) 등에 사용될 수 있는 기본 자원.

### 🔹 **장착 아이템 (Equipable)**

- **도끼 (Axe)** 🪓 : 장착 시 **공격력 증가**.
- **기타 장비 아이템** 🛡️ : 착용 보너스를 제공하며, **아이템 데이터에서 쉽게 설정 가능**.

---

# 자원채취
![파밍](https://github.com/user-attachments/assets/964cdad3-ed24-4e0f-b57f-0758cf814e42)

- 자원은 일정 횟수만큼 채취할 수 있으며, **채취가 완료되면 사라집니다**.
- **플레이어가 도끼를 장착한 상태에서 나무를 공격**하면 일정 타격 후 자원이 획득

```csharp
public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacity;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;

            capacity -= 1;
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }

        if (capacity <= 0)
        {
            Destroy(gameObject);
        }
    }
}
```
