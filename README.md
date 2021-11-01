# Test-Project

For player movement, the camera does follow the local player but when having a network player, the camera stops working for the host but works for the client. To fix this, I went into the clone player for the host and change the target eye to left display and the camera begun to properly work for both the host and client. I think the reason is that the host is using the main camera of the game instead of disabling which it should have been disabled when the game was running.
