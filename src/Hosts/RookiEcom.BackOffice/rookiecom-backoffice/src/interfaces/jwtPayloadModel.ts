import { JwtPayload } from "jwt-decode";

export default interface IJwtPayloadModel extends JwtPayload {
    scope?: string[];
    amr ?: string[];
    client_id ?: string;
    auth_time ?: number;
    idp ?: string;
    name?: string;
    role ?: string;
    sid ?: string;
}