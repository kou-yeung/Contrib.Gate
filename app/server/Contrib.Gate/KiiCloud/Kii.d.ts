// Type definitions for Kii SDK
// Kii cloud : http://jp-cloud.kii.com/
// Base on "Cloud SDK v2.1.21 " ( will update to v2.4.13 )
// http://documentation.kii.com/references/js/storage/latest/index.html

// Kii
declare module KiiCloud {
    interface KiiSite {
        US: string;
        JP: string;
        CN: string;
        SG: string;
    }
    export class Kii {
        //Authenticate as app admin.
        public static authenticateAsAppAdmin(clientId: string, clientSecret: string, callbacks: { success(adminContext: KiiAppAdminContext); failure(error: string, statusCode: number) });
        //Creates a reference to a bucket for this user 
        //The bucket will be created / accessed within this app's scope
        public static bucketWithName(bucketName: string): KiiBucket;
        // Returns access token lifetime in seconds.
        public static getAccessTokenExpiration(): number;
        //Retrieve the current app ID
        public static getAppID(): string;
        //Retrieve the current app key
        public static getAppKey(): string;
        //Kii SDK Build Number
        public static getBuildNumber(): string;
        //Kii SDK Version Number
        public static getSDKVersion(): string;
        //Creates a reference to a group with the given name
        public static groupWithName(groupName: string): KiiGroup;
        //Creates a reference to a group with the given name and a list of default members
        public static groupWithNameAndMembers(groupName: string, members: KiiUser[]): KiiGroup;
        //Initialize the Kii SDK Should be the first Kii SDK action your application makes
        public static initialize(appId: string, appKey: string);
        //Initialize the Kii SDK with a specific URL Should be the first Kii SDK action your application makes
        public static initializeWithSite(appId: string, appKey: string, site: string);
        // Set the access token lifetime in seconds.
        public static setAccessTokenExpiration(expiresIn: number);
    }
}

// KiiACL
declare module KiiCloud {
    export class KiiACL {
        //Get the list of active ACLs associated with this object from the server
        public listACLEntries(callbacks: { success(theACL: KiiACL, theEntries: KiiACLEntry[]); failure(theACL: KiiACL, errorString: string) });
        //Add a KiiACLEntry to the local object, if not already present.
        public putACLEntry(entry: KiiACLEntry);
        //Remove a KiiACLEntry to the local object.
        public removeACLEntry(entry: KiiACLEntry);
        //Save the list of ACLEntry objects associated with this ACL object to the server
        public save(callbacks: { success(theSaveACL: KiiACL); failure(theACL: KiiACL, errorString: string) });
    }
}

// KiiACLEntry
declare module KiiCloud {
    enum KiiACLAction {
        KiiACLBucketActionCreateObjects,
        KiiACLBucketActionQueryObjects,
        KiiACLBucketActionDropBucket,
        KiiACLObjectActionRead,
        KiiACLObjectActionWrite,
    }
    export class KiiACLEntry {
        //Create a KiiACLEntry object with a subject and action The entry will not be applied on the server until the KiiACL object is explicitly saved.
        // T : KiiGroup | KiiUser | KiiAnyAuthenticatedUser | KiiAnonymousUser
        public static entryWithSubject<T>(subject: T, action: KiiACLAction);
        //Get the action that is being permitted / restricted in this entry
        public getAction(): KiiACLAction;
        //Get whether or not the action is being permitted to the subject
        public getGrant(): boolean;
        //Get the subject that is being permitted / restricted in this entry
        // T : KiiUser | KiiGroup
        public getSubject<T>(): T;
        //The action that is being permitted / restricted.
        public setAction(value: KiiACLAction);
        //Set whether or not the action is being permitted to the subject
        public setGrant(value: boolean);
        //The KiiUser or KiiGroup entity that is being permitted / restricted
        // T : KiiUser | KiiGroup
        public setSubject<T>(value: T);
    }
}

// KiiAnonymousUser
declare module KiiCloud {
    export class KiiAnonymousUser { }
}

// KiiAnyAuthenticatedUser
declare module KiiCloud {
    export class KiiAnyAuthenticatedUser { }
}

// KiiAppAdminContext
declare module KiiCloud {
    export class KiiAppAdminContext {
        //Creates a reference to a bucket operated by app admin.
        public bucketWithName(bucketName: string): KiiBucket;
        // Find registered KiiUser with the email.
        public findUserByEmail(email: string, callbacks: { success(adminContext: KiiAppAdminContext, theFoundUser: KiiUser); failure(adminContext: KiiAppAdminContext, anErrorString) });
        // Find registered KiiUser with the phone.
        public findUserByPhone(phone: string, callbacks: { success(adminContext: KiiAppAdminContext, theFoundUser: KiiUser); failure(adminContext: KiiAppAdminContext, anErrorString) });
        //Find registered KiiUser with the user name.
        public findUserByUsername(username: string, callbacks: { success(adminContext: KiiAppAdminContext, theFoundUser: KiiUser); failure(adminContext: KiiAppAdminContext, anErrorString) });
        //Creates a reference to a group operated by app admin using group's ID.
        public groupWithID(groupId: string): KiiGroup;
        //Creates a reference to a group operated by app admin.
        public groupWithName(groupName: string): KiiGroup;
        //Creates a reference to a group operated by app admin using group's URI.
        public groupWithURI(groupUri: string): KiiGroup;
        //Creates a reference to an object operated by app admin using object`s URI.
        public objectWithURI(objectUri: string): KiiObject;
        //Creates a reference to a user operated by app admin.
        public userWithID(userId: string): KiiUser;
    }
}

// KiiBucket
declare module KiiCloud {
    export class KiiBucket {
        //Get the ACL handle for this bucket 
        //Any KiiACLEntry objects added or revoked from this ACL object will be appended to / removed from the server on ACL save.
        public acl(): KiiACL;
        //Execute count aggregation of all clause query on current bucket.
        public count(callbacks: { success(bucket: KiiBucket, query: KiiQuery, count: number); failure(bucket: KiiBucket, query: KiiQuery, errorString: string) });
        //Execute count aggregation of specified query on current bucket.
        public countWithQuery(query: KiiQuery, callbacks: { success(bucket: KiiBucket, query: KiiQuery, count: number); failure(bucket: KiiBucket, query: KiiQuery, errorString: string) });
        //Create a KiiObject within the current bucket 
        //The object will not be created on the server until the KiiObject is explicitly saved.
        public createObject(): KiiObject;
        //Create a KiiObject within the current bucket, specifying its ID.
        public createObjectWithID(objectId: string): KiiObject;
        //The object will not be created on the server until the KiiObject is explicitly saved.
        //Create a KiiObject within the current bucket, with type 
        public createObjectWithType(type: string): KiiObject;
        //Perform a query on the given bucket 
        //The query will be executed against the server, returning a result set.
        public executeQuery(query: KiiQuery, callbacks: { success(queryPerformed, resultSet, nextQuery); failure(queryPerformed, anErrorString) });
        //The name of this bucket
        public getBucketName(): string;
    }
}

// KiiClause
declare module KiiCloud {
    export class KiiClause {
        //Create a KiiClause with the AND operator concatenating multiple KiiClause objects
        public static and(...restOfClause: KiiClause[]);
        //Create an expression of the form(key == value)
        public static equals(key: string, value: any): KiiClause;
        //Create a clause of geo box.
        public static geoBox(key: string, northEast: KiiGeoPoint, southWest: KiiGeoPoint): KiiClause;
        //Create a clause of geo distance.
        public static geoDistance(key: string, center: KiiGeoPoint, radius: number, putDistanceInto: string): KiiClause;
        //Create an expression of the form(key > value)
        public static greaterThan(key: string, value: any);
        //Create an expression of the form(key >= value)
        public static greaterThanOrEqual(key: string, value: any);
        //Create an expression of the form (key in values)
        public static inClause(key: string, values: any[]);
        //Create an expression of the form(key < value)
        public static lessThan(key: string, value: any);
        //Create an expression of the form(key <= value)
        public static lessThanOrEqual(key: string, value: any);
        //Create an expression of the form(key != value)
        public static notEquals(key: string, value: any);
        //Create a KiiClause with the OR operator concatenating multiple KiiClause objects
        public static or(...restOfClause: KiiClause[]);
        //Create an expression of the form(key STARTS WITH value)
        public static startsWith(key: string, value: any);
    }
}

// KiiGeoPoint
declare module KiiCloud {
    export class KiiGeoPoint {
        //Create a geo point with the given latitude and longitude.
        public static geoPoint(latitude: number, longitude: number): KiiGeoPoint;
        //Return the latitide of this point.
        public getLatitude(): number;
        //Return the longitude of this point.
        public getLongitude(): number;
    }
}

// KiiGroup
declare module KiiCloud {
    export class KiiGroup {
        //Adds a user to the given group
        //This method will NOT access the server immediately.
        public addUser(member: KiiUser);
        //Creates a reference to a bucket for this group 
        //The bucket will be created / accessed within this group's scope
        public bucketWithName(bucketName: string): KiiBucket;
        //Updates the group name on the server
        public changeGroupName(newName: string, callbacks: { success(theRenamedGroup: KiiGroup); failure(theGroup: KiiGroup, anErrorString: string) });
        //Returns the owner of this group if this group holds the information of owner.
        public getCachedOwner(): KiiUser;
        //Get the ID of the current KiiGroup instance.
        public getID(): string;
        //Gets a list of all current members of a group
        public getMemberList(callbacks: { success(theGroup: KiiGroup, memberList: KiiUser[]); failure(theGroup: KiiGroup, anErrorString: string, theUsersFailedAdd: KiiUser[], theUsersFailedRemove: KiiUser[]) });
        //The name of this group
        public getName(): string;
        //Gets the owner of the associated group This API does not return all the properties of the owner.
        public getOwner(callbacks: { success(theGroup: KiiGroup, theOwner: KiiUser); failure(theGroup: KiiGroup, anErrorString: string) });
        public getUUID(): string;
        //Instantiate KiiGroup that refers to existing group which has specified ID.
        public static groupWithID(groupId: string): KiiGroup;
        //Creates a reference to a group with the given name
        //Note: Returned instance from this API can not operate existing KiiGroup.
        public static groupWithName(groupName: string): KiiGroup;
        //Creates a reference to a group with the given name and a list of default members
        //Note: Returned instance from this API can not operate existing KiiGroup.
        public static groupWithNameAndMembers(groupName: string, members: KiiUser[]): KiiGroup;
        //Generate a new KiiGroup based on a given URI
        //Note: Returned instance from this API can operate existing KiiGroup.
        public static groupWithURI(uri: string): KiiGroup;
        //Get a specifically formatted string referencing the group 
        //The group must exist in the cloud(have a valid UUID).
        public objectURI(): string;
        //Updates the local group's data with the group data on the server 
        //The group must exist on the server.
        public refresh(callbacks: { success(theRefreshedGroup: KiiGroup); failure(theGroup: KiiGroup, anErrorString: string) });
        //Removes a user from the given group 
        //This method will NOT access the server immediately.
        public removeUser(member: KiiUser);
        //Saves the latest group values to the server 
        //If the group does not yet exist, it will be created.
        public save(callbacks: { success(theSavedGroup: KiiGroup); failure(theGroup: KiiGroup, anErrorString: string) });
    }
}

// KiiObject
declare module KiiCloud {
    export class KiiObject {
        //Delete the object from the server.
        public delete(callbacks: { success(theDeletedObject: KiiObject); failure(obj: KiiObject, anErrorString: string) });
        //Delete the object body from the server.
        public deleteBody(callbacks: { success(obj: KiiObject); failure(obj: KiiObject, anErrorString: string) });
        //Download body data of this object.
        // evt : XMLHttpRequest 'progress' event listener.
        public downloadBody(callbacks: { progress(evt); success(obj: KiiObject, bodyBlob: Blob); failure(obj: KiiObject, anErrorString: string) });
        //Gets the value associated with the given key
        public get<T>(key: string): T;
        //Get the body content - type.
        public getBodyContentType(): string;
        //Get the server's creation date of this object
        public getCreated(): string;
        //Gets the geo point associated with the given key.
        public getGeoPoint(key: string): KiiGeoPoint;
        //Get the modified date of the given object, assigned by the server
        public getKeys(): string[];
        public getModified(): string;
        //Get the application - defined type name of the object
        public getObjectType(): string;
        //Get the UUID of the given object, assigned by the server
        public getUUID(): string;
        //Check if given ID is valid for object ID.
        public static isValidObjectID(objectId: string): boolean;
        //Move KiiObject body from an object to another object.
        public moveBody(targetObjectUri: string, callbacks: { success(theSrcObject: KiiObject, theTgtObjectUri: string); failure(theSrcObject: KiiObject, theTgtObjectUri: string, anErrorString: string) });
        //Get the ACL handle for this file 
        //Any KiiACLEntry objects added or revoked from this ACL object will be appended to / removed from the server on ACL save.
        public objectACL(): KiiACL;
        //Get a specifically formatted string referencing the object 
        //The object must exist in the cloud(have a valid UUID).
        public objectURI(): string;
        //Generate a new KiiObject based on a given URI
        public static objectWithURI(uri: string): KiiObject;
        //Publish object body.
        public publishBody(callbacks: { success(obj: KiiObject, publishedUrl: string); failure(obj: KiiObject, anErrorString: string) });
        //Publish object body with expiration date.
        public publishBodyExpiresAt(expiresAt: Date, callbacks: { success(obj: KiiObject, publishedUrl: string); failure(obj: KiiObject, anErrorString: string) });
        //Publish object body with expiration duration.
        public publishBodyExpiresIn(expiresIn: number, callbacks: { success(obj: KiiObject, publishedUrl: string); failure(obj: KiiObject, anErrorString: string) });
        //Updates the local object's data with the user data on the server 
        //The object must exist on the server.
        public refresh(callbacks: { success(theRefreshedObject: KiiObject); failure(theObject: KiiObject, anErrorString: string) });
        //Create or update the KiiObject on KiiCloud.
        public save(callbacks: { success(theSavedObject: KiiObject); failure(theObject: KiiObject, anErrorString: string) }, overwrite?: boolean);
        //Create or update the KiiObject on KiiCloud.
        public saveAllFields(callbacks: { success(theSavedObject: KiiObject); failure(theObject: KiiObject, anErrorString: string) }, overwrite?: boolean);
        //Sets a key / value pair to a KiiObject 
        //If the key already exists, its value will be written over.
        public set<T>(key: string, value: T);
        //Set Geo point to this object with the specified key.
        public setGeoPoint(key: string, KiiGeoPoint: KiiGeoPoint);
        //Upload body data of this object.
        public uploadBody(srcDataBlob: Blob, callbacks: { progress(evt); success(obj: KiiObject); failure(obj: KiiObject, anErrorString: string) });
    }
}

// KiiQuery
declare module KiiCloud {
    export class KiiQuery {
        //Get the limit of the current query
        public getLimit(): number;
        //Create a KiiQuery object based on a KiiClause
        //By passing null as the 'clause' parameter, all objects can be retrieved.
        public static queryWithClause(clause?: KiiClause): KiiQuery;
        //Set the limit of the given query
        public setLimit(value: number);
        //Set the query to sort by a field in ascending order If a sort has already been set, it will be overwritten.
        public sortByAsc(field: string);
        //Set the query to sort by a field in descending order If a sort has already been set, it will be overwritten.
        public sortByDesc(field: string);
    }
}

// KiiSocialConnect
declare module KiiCloud {
    enum KiiSocialNetworkName {
        FACEBOOK = 1,
        TWITTER = 2,
    }
    export class KiiSocialConnect {
        //Retrieve the current user's access token expiration date from a social network The network must be set up and linked to the current user.
        //Deprecated : Use KiiSocialConnect.getAccessTokenObjectForNetwork instead.
        public static getAccessTokenExpirationForNetwork(networkName: KiiSocialNetworkName): string;
        //Retrieve the current user's access token from a social network The network must be set up and linked to the current user.
        //Deprecated: Use KiiSocialConnect.getAccessTokenObjectForNetwork instead.
        public static getAccessTokenForNetwork(networkName: KiiSocialNetworkName): string;
        //Retrieve the current user's access token object from a social network The network must be set up and linked to the current user.
        public static getAccessTokenObjectForNetwork(networkName: KiiSocialNetworkName): any;
        //Link the currently logged in user with a social network This will initiate the login process for the given network, which for SSO - enabled services like Facebook, will send the user to the Facebook site for authentication.
        public static linkCurrentUserWithNetwork(networkName: KiiSocialNetworkName, options, callbacks: { success(user: KiiUser, network: KiiSocialNetworkName); failure(user: KiiUser, network: KiiSocialNetworkName, anErrorString: string) });
        //Log a user into the social network provided This will initiate the login process for the given network.
        public static logIn(networkName: KiiSocialNetworkName, options, callbacks: { success(user: KiiUser, network: KiiSocialNetworkName); failure(user: KiiUser, network: KiiSocialNetworkName, anErrorString: string) });
        //Set up a reference to one of the supported KiiSocialNetworks.
        public static setupNetwork(networkName: KiiSocialNetworkName, apiKey: string, apiSecret: string, extras: any);
        //Unlink the currently logged in user with a social network The network must already be set up via setupNetwork
        public static unLinkCurrentUserFromNetwork(networkName: KiiSocialNetworkName, callbacks: { success(user: KiiUser, network: KiiSocialNetworkName); failure(user: KiiUser, network: KiiSocialNetworkName, anErrorString: string) });
    }
}

// KiiUser
declare module KiiCloud {
    export class KiiUser {
        //Authenticates a user with the server
        public static authenticate(userIdentifier: string, password: string, callbacks: { success(theAuthedUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) });
        //Asynchronously authenticates a user with the server using a valid access token Authenticates a user with the server.
        public static authenticateWithToken(accessToken: string, callbacks: { success(theAuthedUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) });
        //Creates a reference to a bucket for this user 
        //The bucket will be created / accessed within this user's scope
        public bucketWithName(bucketName: string): KiiBucket;
        //Updates the user's email address on the server
        public changeEmail(newEmail: string, callbacks: { success(theUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) });
        //Updates the user's phone number on the server
        public changePhone(newPhoneNumber: string, callbacks: { success(theUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) });
        //Find registered KiiUser with the email.
        public static findUserByEmail(email: string, callbacks: { success(theFoundUser: KiiUser); failure(anErrorString: string) });
        //Find registered KiiUser with the phone.
        public static findUserByPhone(phone: string, callbacks: { success(theFoundUser: KiiUser); failure(anErrorString: string) });
        //Find registered KiiUser with the user name.
        public static findUserByUsername(username: string, callbacks: { success(theFoundUser: KiiUser); failure(anErrorString: string) });
        //Gets the value associated with the given key
        public get<T>(key: string): T;
        //Get the access token for the user - only available if the user is currently logged in
        public getAccessToken(): string;
        //Get the country code associated with this user
        getCountry(): string;
        //Get the server's creation date of this user
        public getCreated(): string;
        //The currently authenticated user
        public static getCurrentUser(): KiiUser;
        //Get the display name associated with this user
        public getDisplayName(): string;
        //Get the email address associated with this user
        public getEmailAddress(): string;
        //Get the status of the user's email verification.
        public getEmailVerified(): boolean;
        //Get the ID of the current KiiUser instance.
        public getID(): string;
        //Get the modified date of the given user, assigned by the server
        public getModified(): string;
        //Get the phone number associated with this user
        public getPhoneNumber(): string;
        //Get the status of the user's phone number verification.
        public getPhoneVerified(): boolean;
        //Get the username of the given user
        public getUsername(): string;
        public getUUID(): string;
        //Get whether or not the user is pseudo user.
        public isPseudoUser(): boolean;
        //Checks to see if there is a user authenticated with the SDK
        public static loggedIn(): boolean;
        //Logs the currently logged- in user out of the KiiSDK
        public static logOut();
        //Retrieve a list of groups which the user is a member of
        public memberOfGroups(callbacks: { success(theUser: KiiUser, groupList: KiiGroup[]); failure(theUser: KiiUser, anErrorString: string) });
        //Get a specifically formatted string referencing the user 
        //The user must exist in the cloud(have a valid UUID).
        public objectURI(): string;
        //Retrieve the groups owned by this user.
        public ownerOfGroups(callbacks: { success(theUser: KiiUser, groupList: KiiGroup[]); failure(theUser: KiiUser, anErrorString: string) });
        //Sets credentials data and custom fields to pseudo user.
        // for detail : http://documentation.kii.com/references/js/storage/latest/symbols/KiiUser.html#putIdentity
        public putIdentity(identityData: Object, password: string, callbacks: { success(user: KiiUser); failure(user: KiiUser, errorString: string) }, userFields?: Object, removeFields?: Object);
        //Updates the local user's data with the user data on the server 
        //The user must exist on the server.
        public refresh(callbacks: { success(theRefreshedUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) });
        //Registers a user as pseudo user with the server
        public static registerAsPseudoUser(callbacks: { success(theAuthedUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) }, userFields?: Object);
        //Registers a user with the server 
        //The user object must have an associated email / password combination.
        public register(callbacks: { success(theAuthedUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) });
        //Resend the email verification code to the user 
        //This method will re - send the email verification to the currently logged in user
        public resendEmailVerification(callbacks: { success(theUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) });
        //Resend the SMS verification code to the user 
        //This method will re - send the SMS verification to the currently logged in user
        public resendPhoneNumberVerification(callbacks: { success(theUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) });
        //Reset a user's password on the server 
        public static resetPassword(userIdentifier: string, callbacks: { success(); failure(anErrorString: string) });
        //Saves the latest user values to the server 
        //If the user does not yet exist, it will NOT be created.
        public save(callbacks: { success(theSavedUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) });
        //Sets a key / value pair to a KiiUser 
        //If the key already exists, its value will be written over.
        public set<T>(key: string, value: T);
        //Set the country code associated with this user
        public setCountry(value: string);
        //Set the display name associated with this user.
        public setDisplayName(value: string);
        //Update user attributes.
        // for detail : http://documentation.kii.com/references/js/storage/latest/symbols/KiiUser.html#update
        update(identityData: Object, callbacks: { success(user: KiiUser); failure(user: KiiUser, errorString: string) }, userFields?: Object, removeFields?: Object)
        //Update a user's password on the server 
        public updatePassword(fromPassword: string, toPassword: string, callbacks: { success(theUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) });
        //Create a user object to prepare for registration with credentials pre - filled 
        //Creates an pre - filled user object for registration.
        public static userWithCredentials(emailAddress: string, phoneNumber: string, username: string, password: string): KiiUser;
        //Create a user object to prepare for registration with credentials pre - filled 
        //Creates an pre - filled user object for registration.
        public static userWithEmailAddress(emailAddress: string, password: string): KiiUser;
        //Create a user object to prepare for registration with credentials pre - filled 
        //Creates an pre - filled user object for registration.
        public static userWithEmailAddressAndPhoneNumber(emailAddress: string, phoneNumber: string, password: string): KiiUser;
        //Create a user object to prepare for registration with credentials pre - filled 
        //Creates an pre - filled user object for registration.
        public static userWithEmailAddressAndUsername(emailAddress: string, username: string, password: string): KiiUser;
        //Instantiate KiiUser that refers to existing user which has specified ID.
        public static userWithID(userId: string): KiiUser;
        //Create a user object to prepare for registration with credentials pre - filled 
        //Creates an pre - filled user object for registration.
        public static userWithPhoneNumber(phoneNumber: string, password: string): KiiUser;
        //Create a user object to prepare for registration with credentials pre - filled 
        //Creates an pre - filled user object for registration.
        public static userWithPhoneNumberAndUsername(phoneNumber: string, username: string, password: string): KiiUser;
        //Generate a new KiiUser based on a given URI
        public static userWithURI(uri: string): KiiUser;
        //Create a user object to prepare for registration with credentials pre - filled 
        //Creates an pre - filled user object for manipulation.
        public static userWithUsername(username: string, password: string): KiiUser;
        //Verify the current user's phone number 
        //This method is used to verify the phone number of the currently logged in user.
        public verifyPhoneNumber(verificationCode: string, callbacks: { success(theUser: KiiUser); failure(theUser: KiiUser, anErrorString: string) });
    }
}

declare var KiiSite: KiiCloud.KiiSite;
import Kii = KiiCloud.Kii;
import KiiACL = KiiCloud.KiiACL;
import KiiACLAction = KiiCloud.KiiACLAction;
import KiiACLEntry = KiiCloud.KiiACLEntry;
import KiiAnonymousUser = KiiCloud.KiiAnonymousUser;
import KiiAnyAuthenticatedUser = KiiCloud.KiiAnyAuthenticatedUser;
import KiiAppAdminContext = KiiCloud.KiiAppAdminContext;
import KiiBucket = KiiCloud.KiiBucket;
import KiiClause = KiiCloud.KiiClause;
import KiiGeoPoint = KiiCloud.KiiGeoPoint;
import KiiGroup = KiiCloud.KiiGroup;
import KiiObject = KiiCloud.KiiObject;
import KiiQuery = KiiCloud.KiiQuery;
import KiiSocialNetworkName = KiiCloud.KiiSocialNetworkName;
import KiiSocialConnect = KiiCloud.KiiSocialConnect;
import KiiUser = KiiCloud.KiiUser;