# orleans_grainCancellationToken_test

This repository will reproduce an issue with GrainCancellationTokens not working when the work is still queued

## Getting Started

```
dotnet test
# You will see the test fail

cat .\IntegrationTest\bin\Debug\net5\silo*.txt

# 2021-02-18T13:10:28.4374762-05:00  [INF] Recorded opened connection from endpoint "127.0.0.1:44274", client ID "*cli/da05cc82". (1297364f)
# 2021-02-18T13:10:28.5033577-05:00  [INF] Forwarding 1 requests destined for address "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@e72bce8b" to address "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@d45c6d8f" after "Duplicate activation" (f1c9380c)
# 2021-02-18T13:10:28.5089838-05:00  [INF] Trying to forward "NewPlacement Request S127.0.0.1:11111:351367827*cli/da05cc82@26935da8->S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@e72bce8b InvokeMethodRequest Domain.Subject.ITestGrain:LongOperation #13" from "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@e72bce8b" to "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@d45c6d8f" after "Duplicate activation". Attempt 0 (e18e6bbb)
# 2021-02-18T13:10:28.5104335-05:00  [INF] Forwarding 1 requests destined for address "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@b2a4d2a8" to address "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@d45c6d8f" after "Duplicate activation" (f1c9380c)
# 2021-02-18T13:10:28.5104812-05:00  [INF] Trying to forward "NewPlacement Request S127.0.0.1:11111:351367827*cli/da05cc82@26935da8->S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@b2a4d2a8 InvokeMethodRequest Domain.Subject.ITestGrain:LongOperation #14" from "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@b2a4d2a8" to "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@d45c6d8f" after "Duplicate activation". Attempt 0 (e18e6bbb)
# 2021-02-18T13:10:28.5105201-05:00  [INF] Forwarding 1 requests destined for address "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@b977629c" to address "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@d45c6d8f" after "Duplicate activation" (f1c9380c)
# 2021-02-18T13:10:28.5105457-05:00  [INF] Trying to forward "NewPlacement Request S127.0.0.1:11111:351367827*cli/da05cc82@26935da8->S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@b977629c InvokeMethodRequest Domain.Subject.ITestGrain:LongOperation #11" from "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@b977629c" to "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@d45c6d8f" after "Duplicate activation". Attempt 0 (e18e6bbb)
# 2021-02-18T13:10:28.5109160-05:00  [INF] Forwarding 1 requests destined for address "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@f7f465ad" to address "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@d45c6d8f" after "Duplicate activation" (f1c9380c)
# 2021-02-18T13:10:28.5109555-05:00  [INF] Trying to forward "NewPlacement Request S127.0.0.1:11111:351367827*cli/da05cc82@26935da8->S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@f7f465ad InvokeMethodRequest Domain.Subject.ITestGrain:LongOperation #12" from "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@f7f465ad" to "S127.0.0.1:11111:351367827*grn/Domain.Subject.TestGrain/123@d45c6d8f" after "Duplicate activation". Attempt 0 (e18e6bbb)
# 2021-02-18T13:10:28.5802196-05:00  [ERR] Remote token cancellation failed: token with id 9efb6a58-c28f-4880-88ea-f4c41de59a39 was not found (07c93768)
# 2021-02-18T13:10:28.5824937-05:00  [ERR] Remote token cancellation failed: token with id 51063363-5a9c-48fa-bf4c-197a0caee6a1 was not found (2d2e51d5)
# 2021-02-18T13:10:28.5825152-05:00  [ERR] Remote token cancellation failed: token with id d13ad7ff-500c-4f91-ad62-25735e9c9b06 was not found (2eed8761)
# 2021-02-18T13:10:28.5825323-05:00  [ERR] Remote token cancellation failed: token with id 311939e1-f86c-407a-97cc-94a32f69ce2d was not found (06db2ff7)
```
