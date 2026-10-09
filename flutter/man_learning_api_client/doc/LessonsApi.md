# man_learning_api_client.api.LessonsApi

## Load the API package
```dart
import 'package:man_learning_api_client/api.dart';
```

All URIs are relative to *http://localhost:5299*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiLessonsLessonIdGet**](LessonsApi.md#apilessonslessonidget) | **GET** /api/Lessons/{lessonId} | 


# **apiLessonsLessonIdGet**
> LessonResponse apiLessonsLessonIdGet(lessonId)



### Example
```dart
import 'package:man_learning_api_client/api.dart';

final api = ManLearningApiClient().getLessonsApi();
final String lessonId = 38400000-8cf0-11bd-b23e-10b96e4ef00d; // String | 

try {
    final response = api.apiLessonsLessonIdGet(lessonId);
    print(response);
} on DioException catch (e) {
    print('Exception when calling LessonsApi->apiLessonsLessonIdGet: $e\n');
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **lessonId** | **String**|  | 

### Return type

[**LessonResponse**](LessonResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

