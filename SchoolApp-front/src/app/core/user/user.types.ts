import { Status } from '../enums/status.enum';

export interface User {
    id: string;
    fullName: string;
    email: string;
    profileType: ProfileType;
    avatar?: string;
    managedSchoolId?: string;
    isActive: boolean;
}

export interface UserQueryResult {
    id: string;
    fullName: string;
    socialName?: string | null;
    taxDocument: string;
    profileType: ProfileType;
    personType: PersonType;
    avatar?: string | null;
    status: Status;
    gender: Gender;
    birthDay: string;
    address?: string | null;
    phoneNumber?: string | null;
    email: string;
    managedSchoolId?: string | null;
}

export interface UserQueryResultForList {
    id: string;
    fullName: string;
    socialName: string;
    taxDocument: string;
    avatar: string;
    profileType: number;
    status: Status;
    phoneNumber: string;
    email: string;
}

export class CreateUserInput {
    constructor(args?: Partial<CreateUserInput>) {
        Object.assign(this, args);
    }
    fullName: string;
    socialName?: string | null;
    taxDocument: string;
    password: string;
    profileType: ProfileType;
    personType: PersonType;
    avatar?: string | null;
    gender: Gender;
    birthDay: string;
    address?: string | null;
    phoneNumber?: string | null;
    email: string;
    managedSchoolId?: string | null;
}

export interface CreateUserOutput {
    id: string;
}

export class UpdateUserInput {
    constructor(args?: Partial<UpdateUserInput | UserQueryResult | User>) {
        Object.assign(this, args);
    }
    id: string;
    fullName: string;
    socialName?: string | null;
    taxDocument: string;
    profileType: ProfileType;
    personType: PersonType;
    avatar?: string | null;
    status: Status;
    gender: Gender;
    birthDay: string;
    address?: string | null;
    phoneNumber?: string | null;
    email: string;
    managedSchoolId?: string | null;
}

export class UpdatePasswordInput {
    id: string;
    password: string;

    constructor(id: string, password: string) {
        this.id = id;
        this.password = password;
    }
}

export enum PersonType {
    Person = 1,
    Company = 2,
}
export const personTypeOptions = [
    { key: PersonType.Person, value: 'Individual' },
    { key: PersonType.Company, value: 'Company' },
];

export enum ProfileType {
    None,
    Admin,
    SchoolManager,
    Supervisor,
}

export const profileTypeOptions = [
    { key: ProfileType.None, value: 'None' },
    { key: ProfileType.Admin, value: 'Administrator' },
    { key: ProfileType.SchoolManager, value: 'School Manager' },
    { key: ProfileType.Supervisor, value: 'Supervisor' },
];

export enum Gender {
    None,
    Male,
    Female,
    NonBinary,
    PreferNotToDisclose,
    Other,
}

export const genderOptions = [
    { key: Gender.None, value: 'None' },
    { key: Gender.Male, value: 'Male' },
    { key: Gender.Female, value: 'Female' },
    { key: Gender.NonBinary, value: 'Non-Binary' },
    { key: Gender.PreferNotToDisclose, value: 'Prefer Not To Disclose' },
    { key: Gender.Other, value: 'Other' },
];
