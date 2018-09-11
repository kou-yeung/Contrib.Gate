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

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function CreateUser(params, context, done) {

    // 受信データをパースする
    let s = CreateUserSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new CreateUserReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/

