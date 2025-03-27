import { Injectable } from '@angular/core';
import {
    Storage,
    deleteObject,
    getDownloadURL,
    ref,
    uploadBytesResumable,
} from '@angular/fire/storage';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class FireBaseStorageService {
    uploadPercent: Observable<number> | undefined;
    uploadedURL: Observable<string> | undefined;

    constructor(private storage: Storage) {}

    async uploadFile(
        file: File,
        filePath: string,
        fileName = ''
    ): Promise<string> {
        const fileExtension = file.name.split('.').pop();
        const newName = fileName ? `${fileName}.${fileExtension}` : file.name;

        const path = `${filePath}/${newName}`;
        const storageRef = ref(this.storage, path);
        const task = uploadBytesResumable(storageRef, file);

        return new Promise<string>((resolve, reject) => {
            task.on(
                'state_changed',
                (snapshot) => {
                    const progress =
                        (snapshot.bytesTransferred / snapshot.totalBytes) * 100;
                    console.log(`Upload is ${progress}% done`);
                },
                (error) => {
                    reject(error);
                },
                async () => {
                    try {
                        const downloadURL = await getDownloadURL(
                            task.snapshot.ref
                        );
                        resolve(downloadURL);
                    } catch (error) {
                        reject(error);
                    }
                }
            );
        });
    }

    async getLinkFile(fullFileName: string): Promise<string> {
        const storageRef = ref(this.storage, fullFileName);
        return getDownloadURL(storageRef);
    }

    async removeFile(removeURL: string): Promise<void> {
        const storageRef = ref(this.storage, removeURL);
        await deleteObject(storageRef);
    }

    async downloadFile(file: any): Promise<void> {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', file.url, true);
        xhr.responseType = 'blob';

        xhr.onload = function () {
            const urlCreator = window.URL || window.webkitURL;
            const imageUrl = urlCreator.createObjectURL(xhr.response);
            const tag = document.createElement('a');
            tag.href = imageUrl;
            tag.download = file.name;
            document.body.appendChild(tag);
            tag.click();
            document.body.removeChild(tag);
        };

        xhr.send();
    }
}
