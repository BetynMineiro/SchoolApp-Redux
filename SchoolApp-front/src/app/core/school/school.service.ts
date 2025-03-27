import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { FireBaseStorageService } from 'app/services/fireBaseStorage.service';
import {
    ApiResponseBaseModel,
    AppConfig,
    Pagination,
} from 'config/model/app.config.model';
import { lastValueFrom } from 'rxjs';
import {
    CreateSchoolInput,
    CreateSchoolOutput,
    SchoolQueryResult,
    UpdateSchoolInput,
} from './school.types';

@Injectable({
    providedIn: 'root',
})
export class SchoolService {
    private _httpClient = inject(HttpClient);
    private _appConfig = inject(AppConfig);
    private _fireBaseStorageService = inject(FireBaseStorageService);

    getById(id: string) {
        return lastValueFrom(
            this._httpClient.get<ApiResponseBaseModel<SchoolQueryResult>>(
                `${this._appConfig.apiUrl}/school/${id}`
            )
        );
    }

    getList(filterText = '', pageNumber = 1, pageSize = 15) {
        const requestParams = new HttpParams()
            .append('filter', filterText)
            .append('PageSize', pageSize)
            .append('PageNumber', pageNumber);
        return lastValueFrom(
            this._httpClient.get<
                ApiResponseBaseModel<Pagination<SchoolQueryResult>>
            >(`${this._appConfig.apiUrl}/school`, {
                params: requestParams,
            })
        );
    }

    create(request: CreateSchoolInput) {
        return lastValueFrom(
            this._httpClient.post<ApiResponseBaseModel<CreateSchoolOutput>>(
                `${this._appConfig.apiUrl}/school`,
                request
            )
        );
    }

    delete(id: string) {
        return lastValueFrom(
            this._httpClient.delete<ApiResponseBaseModel<any>>(
                `${this._appConfig.apiUrl}/school/${id}`
            )
        );
    }

    update(request: UpdateSchoolInput) {
        return lastValueFrom(
            this._httpClient.put<ApiResponseBaseModel<any>>(
                `${this._appConfig.apiUrl}/school/${request.id}`,
                request
            )
        );
    }

    async uploadAvatar(file: File, school: SchoolQueryResult) {
        const meta = await this._fireBaseStorageService.uploadFile(
            file,
            `school/${school.id}`,
            'avatar'
        );

        const link = await this._fireBaseStorageService.getLinkFile(meta);

        return link;
    }
}
