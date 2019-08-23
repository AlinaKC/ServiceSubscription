import { Role } from "./role";

export class User {
    userId: number;
    username: string;
    password: string;
    firstName: string;
    email: string;
    lastName: string;
    role?: Role;
    token?: string;
    unSubscribe?: boolean;
}