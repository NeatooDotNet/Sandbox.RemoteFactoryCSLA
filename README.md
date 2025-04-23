This is a comparison of CSLA, Neatoo, and DI performance versus standard 'new' and Activator.CreateInstance.


| Method                        | Mean       | Error      | StdDev     | Median     |
|------------------------------ |-----------:|-----------:|-----------:|-----------:|
| CSLABusinessBase              | 730.910 ms | 19.3451 ms | 57.0396 ms | 704.174 ms |
| RemoteFactoryCSLABusinessBase | 446.428 ms |  8.9169 ms | 10.9508 ms | 449.754 ms |
| NeatooEditBase                | 197.414 ms |  3.8079 ms |  8.6725 ms | 196.941 ms |
| DIOnly                        |   3.973 ms |  0.0748 ms |  0.0663 ms |   3.975 ms |
| ConstructorOnly               |   2.496 ms |  0.0490 ms |  0.0583 ms |   2.492 ms |
| ActivatorCreateInstance       |  10.210 ms |  0.1248 ms |  0.1106 ms |  10.203 ms |