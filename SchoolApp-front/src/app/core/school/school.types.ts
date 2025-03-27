import { Status } from '../enums/status.enum';

export interface SchoolQueryResult {
    id: string;
    fullName: string;
    taxDocument: string;
    avatar: string;
    status: Status;
    address: string;
    phoneNumber: string;
    email: string;
}

export interface SchoolQueryListResult {
    id: string;
    fullName: string;
    avatar: string;
    status: Status;
    phoneNumber: string;
    email: string;
}

export class UpdateSchoolInput {
    constructor(args?: Partial<UpdateSchoolInput | SchoolQueryResult>) {
        Object.assign(this, args);
    }
    id: string;
    fullName: string;
    taxDocument: string;
    avatar: string;
    personType: string;
    address: string;
    phoneNumber: string;
    email: string;
}

export class CreateSchoolInput {
    constructor(args?: Partial<CreateSchoolInput>) {
        Object.assign(this, args);
    }
    fullName: string;
    taxDocument: string;
    avatar: string;
    personType: string;
    address: string;
    phoneNumber: string;
    email: string;
}
export interface CreateSchoolOutput {
    id: string;
}
