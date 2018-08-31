//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class CreateUserSend {
    name: string;

	// params -> CreateUserSend
	static Parse(params : any): CreateUserSend {
		return JSON.parse(params.data[0]) as CreateUserSend;
	}
}

class CreateUserReceive {
    step: UserCreateStep;
	
	// CreateUserReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}