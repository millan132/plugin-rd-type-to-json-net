package com.jetbrains.rider.plugins.typetojson

import com.jetbrains.rider.actions.base.RiderAnAction

class SerializeTypeAction : RiderAnAction(
    backendActionId = "SerializeType",
    text = "Serialize to JSON",
    description = "Get the JSON serialized object instance of this type"
)
