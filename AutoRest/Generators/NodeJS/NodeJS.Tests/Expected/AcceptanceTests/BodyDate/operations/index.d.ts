/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for
 * license information.
 * 
 * Code generated by Microsoft (R) AutoRest Code Generator 0.14.0.0
 * Changes may cause incorrect behavior and will be lost if the code is
 * regenerated.
*/

import { ServiceClientOptions, RequestOptions, ServiceCallback } from 'ms-rest';
import * as models from '../models';


/**
 * @class
 * DateModel
 * __NOTE__: An instance of this class is automatically created for an
 * instance of the AutoRestDateTestService.
 */
export interface DateModel {

    /**
     * Get null date value
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getNull(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<Date>): void;
    getNull(callback: ServiceCallback<Date>): void;

    /**
     * Get invalid date value
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getInvalidDate(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<Date>): void;
    getInvalidDate(callback: ServiceCallback<Date>): void;

    /**
     * Get overflow date value
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getOverflowDate(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<Date>): void;
    getOverflowDate(callback: ServiceCallback<Date>): void;

    /**
     * Get underflow date value
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getUnderflowDate(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<Date>): void;
    getUnderflowDate(callback: ServiceCallback<Date>): void;

    /**
     * Put max date value 9999-12-31
     *
     * @param {date} dateBody
     * 
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    putMaxDate(dateBody: Date|string, options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<void>): void;
    putMaxDate(dateBody: Date|string, callback: ServiceCallback<void>): void;

    /**
     * Get max date value 9999-12-31
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getMaxDate(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<Date>): void;
    getMaxDate(callback: ServiceCallback<Date>): void;

    /**
     * Put min date value 0000-01-01
     *
     * @param {date} dateBody
     * 
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    putMinDate(dateBody: Date|string, options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<void>): void;
    putMinDate(dateBody: Date|string, callback: ServiceCallback<void>): void;

    /**
     * Get min date value 0000-01-01
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getMinDate(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<Date>): void;
    getMinDate(callback: ServiceCallback<Date>): void;
}