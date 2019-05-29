# AriesApi library for integarating with ARIES simulation

## How to build?
* Install Visual Studio 2017 (community edition is enough)
* Build project
* Get AriesApi.dll from bin/ directory
* Add it as reference to project
! AriesApi is dependent on RestSharp and NewtonJson libraries

## How to use?

### Initialization
```
AriesApi ariesApi = new AriesApi(URL, PORT);
AriesApiHelper ariesApiHelper = new AriesApiHelper(ariesApi);
```
URL - REST ARIES API host
PORT - REST ARIES API port

### Get Agent list
```
List<Agent> agents = ariesApiHelper.GetAllAgents();
```
### Get Clusters
```
ariesApiHelper.simulation.clusters;
```
