
//import dataAccess from '@/axios-base';

export const submitPrizeDrawEntry = async (prizeDraw) => {

    //return dataAccess.post('/PrizeDraw/SubmitEntry', prizeDraw);    
    
    // Success
    return new Promise(resolve => {

        setTimeout(() => {
            resolve({
                "data": "",
                "status": 200,
                "statusText": "OK",
                "headers": {
                    "content-length": "0"
                },
                "config": {
                    "url": "/PrizeDraw/SubmitEntry",
                    "method": "post",
                    "data": `{"firstName":"${prizeDraw.firstName}","lastName":"${prizeDraw.lastName}","email":"${prizeDraw.email}","serialNumber":"${prizeDraw.serialNumber}","dateOfBirth":"${prizeDraw.dateOfBirth}"}`,
                    "headers": {
                        "Accept": "application/json, text/plain, */*",
                        "Content-Type": "application/json;charset=utf-8"
                    },
                    "baseURL": "https://localhost:5001",
                    "transformRequest": [
                        null
                    ],
                    "transformResponse": [
                        null
                    ],
                    "timeout": 0,
                    "xsrfCookieName": "XSRF-TOKEN",
                    "xsrfHeaderName": "X-XSRF-TOKEN",
                    "maxContentLength": -1,
                    "maxBodyLength": -1
                },
                "request": {}
            })
        }, 5000);
    });
}