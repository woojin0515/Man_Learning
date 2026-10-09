import 'package:flutter/material.dart';

import 'router.dart';

/// Root widget. Material 3 is enabled via [ThemeData.useMaterial3] (the default in current
/// Flutter, kept explicit here for clarity); no custom design system/theme tokens are built in
/// this vertical slice — see ADR-0005/ADR-0009 for the longer-term Flutter UI direction.
class ManLearningApp extends StatelessWidget {
  const ManLearningApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp.router(
      title: 'Man Learning',
      theme: ThemeData(colorSchemeSeed: Colors.indigo, useMaterial3: true),
      routerConfig: appRouter,
    );
  }
}
