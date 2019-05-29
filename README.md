# ARIES-dotnet-lib
ARIES-dotnet-lib is a .NET library which supports interaction with simulator over REST API.

## Features
* Can request current status of simulation and present it as a C# objects.
* Can change the configuration of simulation via posting states.

## API

### Initialization
```c#
AriesApi ariesApi = new AriesApi(URL, PORT);
```
URL - ARIES API host
PORT - ARIES API port (ARIES API default port is 8080)

### Get Simulation
```c#
Simulation simulation = this.ariesApi.GetSimulation();
```

### Get Simulation step
```c#
SimulationStep simulationStep = this.ariesApi.GetSimulationStep();
```

### Get Agent list
```c#
List<Agent> agents = ariesApiHelper.GetAllAgents();
```

### Change simlation configuration
```c#
State state = new State();
state.power_rating = 1000;
state.power_factor = 0.95;
state.incoming_power = 0;
state.battery = new BatteryState();
state.battery.contribution_active = 0.1;
state.battery.contribution_reactive = 0.1;
state.battery.active = 1;

state.electrical_vehicle = new ElectricalVehicleState();
state.electrical_vehicle.contribution_active = 0.3;
state.electrical_vehicle.contribution_reactive = 0.3;
state.electrical_vehicle.power_supplier = 0.3;
state.electrical_vehicle.active = 1;

state.pv_panel = new PVPanelState();
state.pv_panel.solar_irradiance = 1;
state.pv_panel.heating_contribution = 0.3;
state.pv_panel.active = 1;

state.water_tank = new WaterTankState();
state.water_tank.active = 1;

state.wind_generator = new WindGeneratorState();
state.wind_generator.wind_speed = 10;
state.wind_generator.heating_contribution = 0.3;
state.wind_generator.active = 1;

Dictionary<string, State> states = new Dictionary<string, State>();
states.Add("AGENT0", state);

this.ariesApi.PostState(states);
```

## Dependency

ARIES dot net library depends on:
1. Newtonsoft.Json
2. RestSharp
