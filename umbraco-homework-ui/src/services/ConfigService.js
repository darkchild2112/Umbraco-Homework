//import dataAccess from '@/axios-base';



export const getConfig = async () => { 
    
    //return dataAccess.get('/Config');

    return new Promise(resolve => {

        setTimeout(() => {
            resolve({
                "data": {
                    "maxSubmissions": 2,
                    "validation": {
                        "firstNameRules": [
                            {
                                "regex": "\\S",
                                "errorMessage": "First name is mandatory"
                            }
                        ],
                        "lastNameRules": [
                            {
                                "regex": "\\S",
                                "errorMessage": "Last name is mandatory"
                            }
                        ],
                        "emailRules": [
                            {
                                "regex": "\\S",
                                "errorMessage": "Email address is mandatory"
                            },
                            {
                                "regex": "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:.[a-zA-Z0-9-]+)*$",
                                "errorMessage": "Email address needs to be in the format of xxx@xxx.xxx"
                            }
                        ],
                        "serialNumberRules": [
                            {
                                "regex": "\\S",
                                "errorMessage": "Serial number is mandatory"
                            },
                            {
                                "regex": "^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
                                "errorMessage": "The Serial number is in an incorrect format"
                            }
                        ],
                        "dateOfBirthRules": [
                            {
                                "regex": "\\S",
                                "errorMessage": "Date of birth is mandatory"
                            },
                            {
                                "regex": "(0?[1-9]|[12][0-9]|3[01])[- \\/.](0?[1-9]|1[012])[- \\/.](19|20)\\d\\d",
                                "errorMessage": "The date of birth needs to be in the format of dd/mm/yyyy"
                            }
                        ]
                    }
                },
                "status": 200,
                "statusText": "OK",
                "headers": {
                    "content-type": "application/json; charset=utf-8"
                },
                "config": {
                    "url": "/Config",
                    "method": "get",
                    "headers": {
                        "Accept": "application/json, text/plain, */*"
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
};