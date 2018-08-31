//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class ServerDebugSend {


	// params -> ServerDebugSend
	static Parse(params : any): ServerDebugSend {
		return JSON.parse(params.data[0]) as ServerDebugSend;
	}
}

class ServerDebugReceive {
    param: string;
    context: string;
	
	// ServerDebugReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}