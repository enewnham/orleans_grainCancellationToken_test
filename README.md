# orleans_grainCancellationToken_test

This repository will reproduce an issue with GrainCancellationTokens not working when the work is still queued

## Getting Started

```
dotnet test
# You will see the test fail

cat .\IntegrationTest\bin\Debug\net5\silo*.txt

# In the log output you will see 4 out of 5 failed to cancel properly

# 2021-02-18T14:26:06.1615139-05:00  [INF] Recorded opened connection from endpoint "127.0.0.1:46194", client ID "*cli/8b7eaebc". (1297364f)
# 2021-02-18T14:26:06.2421583-05:00  [INF] Start LongOperation 00:00:00.2000000 (eeb147e4)
# 2021-02-18T14:26:06.3537301-05:00  [ERR] Remote token cancellation failed: token with id 6cdc4a94-aeca-45e4-9550-e5dcf7c641f6 was not found (aaf2fb6d)
# 2021-02-18T14:26:06.3548523-05:00  [ERR] Remote token cancellation failed: token with id 38161c4b-dc63-4783-99ce-584da3d6ff58 was not found (bb6045dd)
# 2021-02-18T14:26:06.3548719-05:00  [ERR] Remote token cancellation failed: token with id f860648e-ae54-46c1-a46f-4125342f686f was not found (0cb37435)
# 2021-02-18T14:26:06.3548873-05:00  [ERR] Remote token cancellation failed: token with id ec17c17c-9eb1-42ae-babd-66e0ffe9b96b was not found (9fd40602)
# 2021-02-18T14:26:06.3549238-05:00  [INF] Start LongOperation 00:00:00.2000000 (eeb147e4)
# 2021-02-18T14:26:06.5657431-05:00  [INF] End LongOperation (31b3af91)
# 2021-02-18T14:26:06.5659011-05:00  [INF] Start LongOperation 00:00:00.2000000 (eeb147e4)
# 2021-02-18T14:26:06.7686912-05:00  [INF] End LongOperation (31b3af91)
# 2021-02-18T14:26:06.7688783-05:00  [INF] Start LongOperation 00:00:00.2000000 (eeb147e4)
# 2021-02-18T14:26:06.9723506-05:00  [INF] End LongOperation (31b3af91)
# 2021-02-18T14:26:06.9725037-05:00  [INF] Start LongOperation 00:00:00.2000000 (eeb147e4)
# 2021-02-18T14:26:07.1774437-05:00  [INF] End LongOperation (31b3af91)
```
