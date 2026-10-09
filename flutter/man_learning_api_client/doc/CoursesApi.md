# man_learning_api_client.api.CoursesApi

## Load the API package
```dart
import 'package:man_learning_api_client/api.dart';
```

All URIs are relative to *http://localhost:5299*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiCoursesCourseIdGet**](CoursesApi.md#apicoursescourseidget) | **GET** /api/Courses/{courseId} | 
[**apiCoursesGet**](CoursesApi.md#apicoursesget) | **GET** /api/Courses | 


# **apiCoursesCourseIdGet**
> CourseDetailResponse apiCoursesCourseIdGet(courseId)



### Example
```dart
import 'package:man_learning_api_client/api.dart';

final api = ManLearningApiClient().getCoursesApi();
final String courseId = 38400000-8cf0-11bd-b23e-10b96e4ef00d; // String | 

try {
    final response = api.apiCoursesCourseIdGet(courseId);
    print(response);
} on DioException catch (e) {
    print('Exception when calling CoursesApi->apiCoursesCourseIdGet: $e\n');
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **courseId** | **String**|  | 

### Return type

[**CourseDetailResponse**](CourseDetailResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **apiCoursesGet**
> BuiltList<CourseResponse> apiCoursesGet()



### Example
```dart
import 'package:man_learning_api_client/api.dart';

final api = ManLearningApiClient().getCoursesApi();

try {
    final response = api.apiCoursesGet();
    print(response);
} on DioException catch (e) {
    print('Exception when calling CoursesApi->apiCoursesGet: $e\n');
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**BuiltList&lt;CourseResponse&gt;**](CourseResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

