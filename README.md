
# CBORModular

**CBORModular** je projekt zaměřený na práci s CBOR (Concise Binary Object Representation) daty. Tento projekt poskytuje nástroje pro serializaci a deserializaci datových struktur ve formátu CBOR a umožňuje emulovat komunikaci přes Bluetooth Low Energy (BLE).

## Použití

### Nastavení simulace Bluetooth

Nejprve vytvořte instanci simulátoru Bluetooth a procesoru dat:

```csharp
var bluetoothSimulator = new BluetoothSimulator();
var dataProcessor = new DataProcessor(bluetoothSimulator);
```

### Přidání dat s typovou bezpečností

Přidávejte data ručně do úložiště dat s typovou bezpečností:

```csharp
DataStorage.AddData(RequestDataIdentifier.Speed, 15.5f);
```

### Přidání parametrů pro Bluetooth požadavky

Nastavte parametry, které chcete požadovat přes Bluetooth:

```csharp
DataStorage.AddRequest(RequestDataIdentifier.Speed, RequestDataIdentifier.Throttle);
DataStorage.AddRequest(RequestDataIdentifier.AverageSpeed, SetDataIdentifier.HandBrake);
```

### Emulace požadavku přes Bluetooth

Pro emulaci požadavku přes Bluetooth a resetování úložiště požadavků:

```csharp
dataProcessor.ProcessBluetooth(MessageType.Request);
```

### Přidání seznamu požadovaných identifikátorů

Můžete také přidat seznam identifikátorů:

```csharp
var requestedIdentifiers = new List<RequestDataIdentifier>
{
    RequestDataIdentifier.Speed,
    RequestDataIdentifier.GForce,
    RequestDataIdentifier.EngineRPM,
    RequestDataIdentifier.LightLevel,
    RequestDataIdentifier.Gear
};

DataStorage.AddRequest(requestedIdentifiers.ToArray());
```

### Opakování požadavku

Odešlete další simulovaný požadavek přes Bluetooth:

```csharp
dataProcessor.ProcessBluetooth(MessageType.Request);
```



### Nastavení hodnot por Set požadavky

```csharp
DataStorage.AddSet(SetDataIdentifier.ABS, true);
dataProcessor.ProcessBluetooth(MessageType.Set);

```

### Zobrazení získaných dat v UI

Data uložená v `DataStorage` můžete zobrazit v uživatelském rozhraní:

```csharp
SpeedLabel.Text = $"Speed: {DataStorage.GetLastValue(RequestDataIdentifier.Speed)} km/h";
ThrottleLabel.Text = $"Throttle: {DataStorage.GetLastValue(RequestDataIdentifier.Throttle)}%";
```

Pro vypsání času

```csharp
TimeLabel.Text = $"Time: {DataStorage.GetLastValue(RequestDataIdentifier.Speed, entry => entry.Timestamp)}";
```

---
