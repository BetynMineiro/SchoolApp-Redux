import { Pagination } from 'config/model/app.config.model';
import { User, UserQueryResult, UserQueryResultForList } from '../user.types';

export interface UserState {
    user: User | UserQueryResult | null;
    userList: Pagination<UserQueryResultForList> | null;
    loading: boolean;
    error: any;
}

export const initialState: UserState = {
    user: null,
    userList: null,
    loading: false,
    error: null,
};
