
# CBORModular

**CBORModular** je projekt zaměřený na práci s CBOR (Concise Binary Object Representation) daty. Tento projekt poskytuje nástroje pro serializaci a deserializaci datových struktur ve formátu CBOR a emulaci BLE komunikace.

## Použití

### Serializace a deserializace CBOR dat

Ukázka serializace a deserializace pomocí `CborService`:

```csharp
var data = new YourDataModel { /* inicializace dat */ };
var cborData = CborService.Serialize(data);
var deserializedData = CborService.Deserialize<YourDataModel>(cborData);
```

### Emulace BLE požadavků a odpovědí

Použití BLE emulátoru pro odeslání požadavku a získání odpovědi:

```csharp
var request = CborService.CreateRequest(new[] { "Speed", "Throttle" });
var response = BleEmulator.SendRequest(request);
```

## Struktura projektu

- **DataModels** - Třídy definující datové modely pro CBOR.
- **Services** - Služby pro práci s CBOR daty a emulaci BLE komunikace.
